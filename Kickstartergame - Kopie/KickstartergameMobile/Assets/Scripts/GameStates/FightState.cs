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
        private UIController uiController;
        private UnitsController unitController;
        private int attackCount;
        private string startMusic;
        private AttackReaction reaction;
        private DefenseType defense;
        private bool isDefense;
        private bool react;

        public FightState(LivingObject attacker, LivingObject defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
            Debug.Log("FightState " + attacker.Name + " " + defender.Name);
            uiController = MainScript.GetInstance().GetController<UIController>();
            unitController = MainScript.GetInstance().GetController<UnitsController>();
        }
        
        #region GameState
        public override void enter()
        {
            react = false;
            Debug.Log(attacker.GameTransform.GetRotation());
            Debug.Log(defender.GameTransform.GetRotation());
            ShowFightUI();
            unitController.HideUnits();
            EventContainer.startAttack += DoAttack;
            EventContainer.counterClicked = CounterClicked;
            EventContainer.dodgeClicked = DodgeClicked;
            EventContainer.guardClicked = GuardClicked;
            SetUpMusic();
        }
        public override void update()
        {
        }
        public override void exit()
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
            EventContainer.attacktButtonCLicked -= EndFight;
            EventContainer.startAttack -= DoAttack;
            GameObject.FindObjectOfType<AudioManager>().ChangeMusic(startMusic, "Fight", true);
        }
        #endregion

        #region Eventtriggered Methods
        private void DoAttack(AttackType attackType, TargetPoint attackTarget)
        {
            attackCount--;
            if (attackCount >= 0)
                MainScript.GetInstance().StartCoroutine(Attack(attackType, attackTarget));
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
            startMusic = GameObject.FindObjectOfType<AudioManager>().GetCurrentlyPlayedMusicTracks()[0];
            GameObject.FindObjectOfType<AudioManager>().ChangeMusic("Fight", startMusic);
        }
        private bool DoesAttackHit(LivingObject attacker, LivingObject defender, AttackType attackType, TargetPoint attackTarget)
        {
            int hit = attacker.BattleStats.GetHitAgainstTarget(defender);
            if (defense != null)
                hit += defense.Hit;
            if (attackType != null)
                hit += attackType.Hit;
            if (attackTarget != null)
                hit += attackTarget.HIT_INFLUENCE;
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
        private void SingleAttack(LivingObject attacker, LivingObject defender, AttackType attackType,TargetPoint attackTarget)
        {
            if (DoesAttackHit(attacker, defender, attackType, attackTarget))
            {
                List<float> attackModifier = new List<float>();
                if (attackType!=null)
                    attackModifier.Add(attackType.DamageMultiplier);
                if(attackTarget !=null)
                    attackModifier.Add(attackTarget.DamageMultiplier);
                if (defense != null)
                    attackModifier.Add(defense.Atk_Mult);
                
                if (attackType.Name == "SpecialAttack")
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
                            uiController.reactUIController.ShowCounterDamageText(damage);
                        else
                            uiController.attackUIController.ShowDamageText(damage);
                    }
                    if (defender.Player.IsHumanPlayer)
                    {
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
        IEnumerator Attack(AttackType attackType, TargetPoint attackTarget)
        {
            yield return new WaitForSeconds(ATTACK_DELAY);
            SingleAttack(attacker, defender, attackType, attackTarget);
            if (react && attackCount == 0 && defender is Monster)
            {
                Debug.Log("Start Reaction!");
                yield return new WaitForSeconds(1.5f);
                Monster m = (Monster)defender;
                reaction = m.GetRandomAttackReaction();
                reaction.TargetPositions.Add(attacker.GridPosition.GetPos());
                uiController.attackUIController.ShowAttackReaction(defender.Name, reaction.Name);
                EventContainer.reactionFinished += EndFight;
                EventContainer.continuePressed += ExecuteReaction;

                yield break;
            }
            if (defense!=null && defense.Name=="Counter")
            {
                yield return new WaitForSeconds(3.0f);
                SingleAttack(defender, attacker, null,null);
                yield return new WaitForSeconds(1.0f);
            }
            if (attackCount == 0)
                EndFight();
        }
    }
}
