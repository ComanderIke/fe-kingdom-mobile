using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Battle;
using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.AI.AttackReactions;
using Assets.Scripts.Characters.Monsters;

namespace Assets.Scripts.GameStates
{
    class FightState : GameState
    {
        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.0f;

        private LivingObject attacker;
        private LivingObject defender;
        private UISystem uiController;
        private UnitsSystem unitController;
        private int attackCount;
        private string startMusic;
        private AttackReaction reaction;
        private DefenseType defense;
        private bool isDefense;
        private bool react;
        private bool frontAttack;
        private bool surpriseAttack;

        public FightState(LivingObject attacker, LivingObject defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            frontAttack = false;
            surpriseAttack = false;
            if (attacker.BattleStats.IsFrontalAttack(defender))
            {
                frontAttack = true;
            }
            else if (attacker.BattleStats.IsBackSideAttack(defender))
            {
                surpriseAttack = true;
            }
            attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
            Debug.Log("FightState " + attacker.Name + " " + defender.Name);
            uiController = MainScript.GetInstance().GetSystem<UISystem>();
            unitController = MainScript.GetInstance().GetSystem<UnitsSystem>();
        }
        
        #region GameState
        public override void Enter()
        {
            react = false;
           
            ShowFightUI();
            unitController.HideUnits();
            EventContainer.startAttack += DoAttack;
            EventContainer.counterClicked = CounterClicked;
            EventContainer.dodgeClicked = DodgeClicked;
            EventContainer.guardClicked = GuardClicked;
            SetUpMusic();
        }
        public override void Update()
        {
        }
        public override void Exit()
        {
            //CameraMovement.locked = false;
            uiController.ShowMapUI();
            uiController.HideFightUI();
            uiController.HideReactUI();
            unitController.ShowUnits();
            if (!attacker.IsAlive())
            {
                attacker.Die();
            }
            if (!defender.IsAlive())
            {
                defender.Die();
            }
            EventContainer.startAttack -= DoAttack;
            MainScript.GetInstance().GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
        }
        #endregion

        #region Eventtriggered Methods
        private void DoAttack(AttackType attackType)
        {
            attackCount--;
            if (attackCount >= 0)
                MainScript.GetInstance().StartCoroutine(Attack(attackType));
        }
        private void CounterClicked()
        {
            Human human = (Human)defender;
            defense = human.DefenseTypes.Find(a => a.Name == "Counter");
        }
        private void DodgeClicked()
        {
            Human human = (Human)defender;
            defense = human.DefenseTypes.Find(a => a.Name == "Dodge");
        }
        private void GuardClicked()
        {
            Human human = (Human)defender;
            defense = human.DefenseTypes.Find(a => a.Name == "Guard");
        }
        #endregion

        private void ShowFightUI()
        {
            uiController.HideMapUI();
            if (attacker.Player.IsHumanPlayer)
            {
                isDefense = false;
                uiController.ShowFightUI(attacker, defender);
            }
            else if (defender.Player.IsHumanPlayer)
            {
                isDefense = true;
                uiController.ShowReactUI(attacker, defender);
            }
        }
        private void SetUpMusic()
        {
            startMusic = MainScript.GetInstance().GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
            MainScript.GetInstance().GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
        }
        private bool DoesAttackHit(LivingObject attacker, LivingObject defender, AttackType attackType)
        {
            int hit = attacker.BattleStats.GetHitAgainstTarget(defender);
            if (defense != null)
                hit += defense.Hit;
            if (attackType != null)
                hit += attackType.Hit;
            if (surpriseAttack)
                hit += attacker.BattleStats.SurpriseAttackBonusHit;
            Debug.Log(hit);
            return UnityEngine.Random.Range(1, 101) <= hit;
        }
        private void EndFight()
        {
            MainScript.GetInstance().StartCoroutine(End());
        }
        private void ExecuteReaction()
        {
            EventContainer.continuePressed -= ExecuteReaction;
            reaction.Execute();
        }
        private void SingleAttack(LivingObject attacker, LivingObject defender, AttackType attackType)
        {
            if (DoesAttackHit(attacker, defender, attackType))
            {
                List<float> attackModifier = new List<float>();
                if (attackType!=null)
                    attackModifier.Add(attackType.DamageMultiplier);
                if(frontAttack)
                    attackModifier.Add(attacker.BattleStats.FrontalAttackModifier);
                if (defense != null)
                    attackModifier.Add(defense.DamageMultiplier);
                
                if (attackType != null&&attackType.Name == "SpecialAttack")
                {
                    ((Human)attacker).SpecialAttackManager.equippedSpecial.UseSpecial(attacker, attacker.BattleStats.GetDamage(attackModifier), defender);
                }
                else
                {
                    int damage = defender.InflictDamage(attacker.BattleStats.GetDamage(attackModifier), attacker);
                    if (attacker.Player.IsHumanPlayer)
                    {
                        react = true;
                        if (isDefense)
                        {
                            GameObject.FindObjectOfType<EnemySpriteController>().StartAttackAnimation();
                            GameObject.FindObjectOfType<AllySpriteController>().ShakeAnimation(10 + 1f * damage);
                            GameObject.FindObjectOfType<AllySpriteController>().StartBlinkAnimation();
                            uiController.reactUIController.ShowCounterDamageText(damage);
                        }
                        else
                        {
                            GameObject.FindObjectOfType<AllySpriteController>().StartAttackAnimation();
                            GameObject.FindObjectOfType<EnemySpriteController>().ShakeAnimation(10 + 1f * damage);
                            GameObject.FindObjectOfType<EnemySpriteController>().StartBlinkAnimation();
                            uiController.attackUIController.ShowDamageText(damage);
                        }
                    }
                    if (defender.Player.IsHumanPlayer)
                    {
                        GameObject.FindObjectOfType<AllySpriteController>().StartAttackAnimation();
                        GameObject.FindObjectOfType<EnemySpriteController>().ShakeAnimation(10 + 1f * damage);
                        GameObject.FindObjectOfType<EnemySpriteController>().StartBlinkAnimation();
                        uiController.reactUIController.ShowDamageText(damage);
                    }
                }
            }
            else
            {
                if (attacker.Player.IsHumanPlayer)
                {
                    if(isDefense)
                        uiController.reactUIController.ShowCounterMissText();
                    else
                        uiController.attackUIController.ShowMissText();
                }
                if (defender.Player.IsHumanPlayer)
                {
                    uiController.reactUIController.ShowMissText();
                }
            }
            
        }

        IEnumerator End()
        {
            yield return new WaitForSeconds(1.0f);
            EventContainer.commandFinished -= EndFight;
            EventContainer.continuePressed = null;
            MainScript.GetInstance().SwitchState(new GameplayState());
            attacker.UnitTurnState.UnitTurnFinished();
            EventContainer.commandFinished();
        }
        IEnumerator Attack(AttackType attackType)
        {
            yield return new WaitForSeconds(ATTACK_DELAY);
            SingleAttack(attacker, defender, attackType);
            if (!defender.IsAlive())
            {
                EndFight();
                yield break;
            }
            int reactionChance = 50;
            if (frontAttack)
                reactionChance = 100;
            if (surpriseAttack)
                reactionChance = 0;
            if (UnityEngine.Random.Range(1, 101) <= reactionChance&& react && attackCount == 0 && defender is Monster)
            {
                Debug.Log("Start Reaction!");
                yield return new WaitForSeconds(1.5f);
                Monster m = (Monster)defender;
                
                reaction = m.GetRandomAttackReaction();
                reaction.TargetPositions.Add(attacker.GridPosition.GetPos());
                //uiController.attackUIController.ShowAttackReaction(defender.Name, reaction.Name);
                EventContainer.reactionFinished += EndFight;
                EventContainer.continuePressed += ExecuteReaction;

                yield break;
            }
            if (defense!=null && defense.Name=="Counter")
            {
                yield return new WaitForSeconds(3.0f);
                SingleAttack(defender, attacker, null);
                yield return new WaitForSeconds(1.0f);
            }
            if (attackCount == 0)
                EndFight();
        }
    }
}
