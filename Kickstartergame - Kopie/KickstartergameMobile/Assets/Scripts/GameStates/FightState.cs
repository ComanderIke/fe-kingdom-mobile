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
        private UnitController unitController;

        public FightState(LivingObject attacker, LivingObject defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            uiController = GameObject.FindObjectOfType<UIController>();
            unitController = GameObject.FindObjectOfType<UnitController>();
        }
        
        public override void enter()
        {
            //CameraMovement.locked = true;
            Debug.Log("FIGHT" + attacker.Name + " " + defender.Name);
            uiController.ShowFightUI(attacker, defender);
            unitController.HideUnits();
            UIController.attacktButtonCLicked += DoAttack;
        }

        public override void update()
        {
        }

        public override void exit()
        {
            //CameraMovement.locked = false;
            uiController.HideFightUI();
            unitController.ShowUnits();
            UIController.attacktButtonCLicked -= EndFight;
            UIController.attacktButtonCLicked -= DoAttack;
        }

        private bool DoesAttackHit(LivingObject attacker, LivingObject defender)
        {
           return attacker.BattleStats.GetHitAgainstTarget(defender) <= UnityEngine.Random.Range(1, 101);
        }

        private void DoAttack()
        {
            MainScript.GetInstance().StartCoroutine(Attack());
            MainScript.GetInstance().StartCoroutine(End());
        }

        private void EndFight()
        {
            MainScript.GetInstance().SwitchState(new GameplayState());
            Debug.Log("Fight Finished!");
            EventContainer.commandFinished();
        }

        IEnumerator Attack()
        {
            yield return new WaitForSeconds(ATTACK_DELAY);
            if (DoesAttackHit(attacker, defender))
            {
                defender.InflictDamage(attacker.BattleStats.GetDamage(), attacker);
            }
            else
            {
                uiController.attackUIController.ShowMissText();
            }
        }

        IEnumerator End()
        {
            yield return new WaitForSeconds(FIGHT_TIME);
            EndFight();
        }
       
    }
}
