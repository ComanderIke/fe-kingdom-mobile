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
            attackerBonusDmg = 0;
            attackerHit = attacker.BattleStats.GetHitAgainstTarget(defender);
            uiController.ShowFightUI(attacker, defender);
            unitController.HideUnits();
            UIController.attacktButtonCLicked += DoAttack;
            EventContainer.attackerDmgChanged += AttackerDmgChanged;
            EventContainer.attackerHitChanged += AttackerHitChanged;
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

        private bool DoesAttackHit(LivingObject attacker, LivingObject defender)
        {
            int rng = UnityEngine.Random.Range(1, 101);
           return rng <= attackerHit;
        }

        private void DoAttack()
        {
            MainScript.GetInstance().StartCoroutine(Attack());
            MainScript.GetInstance().StartCoroutine(End());
        }

        private void EndFight()
        {
            MainScript.GetInstance().SwitchState(new GameplayState());
            attacker.UnitTurnState.IsActive = false;
            EventContainer.commandFinished();
        }

        IEnumerator Attack()
        {
            yield return new WaitForSeconds(ATTACK_DELAY);
            if (DoesAttackHit(attacker, defender))
            {
                Debug.Log("ATTACK " + defender.Name);
                Debug.Log(attacker.Name+" "+attacker.BattleStats.GetDamageAgainstTarget(defender));
                Debug.Log(attackerBonusDmg);
                defender.InflictDamage(attacker.BattleStats.GetDamage() + attackerBonusDmg, attacker);
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
