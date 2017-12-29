using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Battle;
using Assets.Scripts.Characters;
using Assets.Scripts.Events;

namespace Assets.Scripts.GameStates
{
    class FightState : GameState
    {
        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.25f;

        private LivingObject attacker;
        private LivingObject defender;
        private UIController uiController;
        private UnitsController unitController;
        private int attackerBonusDmg;
        private int attackerHit;
        private bool counter;

        public FightState(LivingObject attacker, LivingObject defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            Debug.Log("FightState " + attacker.Name + " " + defender.Name);
            uiController = MainScript.GetInstance().GetController<UIController>();
            unitController = MainScript.GetInstance().GetController<UnitsController>();
        }
        
        public override void enter()
        {
            //CameraMovement.locked = true;
            counter = false;
            attackerBonusDmg = 0;
            attackerHit = attacker.BattleStats.GetHitAgainstTarget(defender);
            if(attacker.Player.IsHumanPlayer)
                uiController.ShowFightUI(attacker, defender);
            if(defender.Player.IsHumanPlayer)
                uiController.ShowReactUI(attacker, defender);
            unitController.HideUnits();
            UIController.attacktButtonCLicked += DoAttack;
            EventContainer.attackerDmgChanged += AttackerDmgChanged;
            EventContainer.attackerHitChanged += AttackerHitChanged;
            EventContainer.counterClicked = CounterClicked;
            EventContainer.dodgeClicked = DodgeClicked;
            EventContainer.guardClicked = GuardClicked;
        }
        int counterBonusAttack = 0;
        int counterBonusHit = 0;
        void CounterClicked(int attack, int hit)
        {
            counterBonusAttack = attack;
            counterBonusHit = hit * 10;
            counter = true;
        }
        void DodgeClicked(int dodge)
        {
            attackerHit -= 20 + dodge * 10;
        }
        void GuardClicked(int guard)
        {
            attackerHit += 20;
            attackerBonusDmg -= 2 + guard * 1;
        }
        void AttackerDmgChanged(int bonusDamage)
        {
            attackerBonusDmg = bonusDamage;
        }
        void AttackerHitChanged(int hit)
        {
            attackerHit = hit;
        }
        public override void update()
        {
        }

        public override void exit()
        {
            //CameraMovement.locked = false;
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
            UIController.attacktButtonCLicked -= EndFight;
            UIController.attacktButtonCLicked -= DoAttack;
        }

        private bool DoesAttackHit(LivingObject attacker, LivingObject defender, bool counter)
        {
            if (!counter)
                Debug.Log("Hitchance: " + attackerHit);
            else
                Debug.Log("Hitchance: " +(attacker.BattleStats.GetHitAgainstTarget(defender) + counterBonusHit));
           
            int rng = UnityEngine.Random.Range(1, 101);
            if (!counter)
                return rng <= attackerHit;
            else
                return rng <= (attacker.BattleStats.GetHitAgainstTarget(defender) + counterBonusHit);
        }
        private void DoAttack()
        {
            MainScript.GetInstance().StartCoroutine(Attack());
            
        }

        private void EndFight()
        {
            Debug.Log("Fight Finished!");
            MainScript.GetInstance().SwitchState(new GameplayState());
            attacker.UnitTurnState.UnitTurnFinished();
            EventContainer.commandFinished();
        }

        IEnumerator Attack()
        {
            yield return new WaitForSeconds(ATTACK_DELAY);
            SingleAttack(attacker, defender, false);
            
            if (counter)
            {
                yield return new WaitForSeconds(3.0f);
                SingleAttack(defender, attacker, true);
            }
            yield return new WaitForSeconds(3.5f);
            EndFight();
        }
        private void SingleAttack(LivingObject attacker, LivingObject defender, bool counter)
        {
            if (DoesAttackHit(attacker, defender, counter))
            {
                if (attacker.Player.IsHumanPlayer&&!counter)
                    uiController.attackUIController.ShowDamageText(defender.InflictDamage(attacker.BattleStats.GetDamage() + attackerBonusDmg, attacker));
                if (defender.Player.IsHumanPlayer || counter)
                {
                    if (!counter)
                        uiController.reactUIController.ShowDamageText(defender.InflictDamage(attacker.BattleStats.GetDamage() + attackerBonusDmg, attacker));
                    else
                        uiController.reactUIController.ShowCounterDamageText(defender.InflictDamage(attacker.BattleStats.GetDamage() + counterBonusAttack, attacker));
                }
                    
            }
            else
            {
                if (attacker.Player.IsHumanPlayer && !counter)
                    uiController.attackUIController.ShowMissText();
                if (defender.Player.IsHumanPlayer || counter)
                {
                    if(!counter)
                        uiController.reactUIController.ShowMissText();
                    else
                        uiController.reactUIController.ShowCounterMissText();
                }
            }
        }

       
    }
}
