using Assets.Core;
using Assets.GameActors.Units;
using Assets.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Mechanics
{
    public class BattleSystem : IEngineSystem
    {
        public delegate void OnStartAttackEvent();

        public static OnStartAttackEvent OnStartAttack;

        public delegate void OnAllyTakesDamageEvent(int damage, bool magic = false);

        public static OnAllyTakesDamageEvent OnAllyTakesDamage;

        public delegate void OnEnemyTakesDamageEvent(int damage, bool magic = false);

        public static OnEnemyTakesDamageEvent OnEnemyTakesDamage;

        private const float FIGHT_TIME = 3.8f;
        private const float ATTACK_DELAY = 0.0f;
        private readonly Unit attacker;
        private readonly Unit defender;
        private readonly bool frontAttack;
        private readonly bool surpriseAttack;
        private int attackCount;
        private readonly UiSystem uiController;

        public BattleSystem(Unit attacker, Unit defender)
        {
            this.attacker = attacker;
            this.defender = defender;
            uiController = MainScript.Instance.GetSystem<UiSystem>();
            if (attacker.BattleStats.IsFrontalAttack(defender))
            {
                frontAttack = true;
            }
            else if (attacker.BattleStats.IsBackSideAttack(defender))
            {
                surpriseAttack = true;
            }

            attackCount = attacker.BattleStats.GetAttackCountAgainst(defender);
        }

        public void DoAttack()
        {
            attackCount--;
            if (attackCount >= 0)
                MainScript.Instance.StartCoroutine(Attack());
        }

        private List<float> GetAttackModifiers()
        {
            var attackModifiers = new List<float>();
            if (frontAttack)
                attackModifiers.Add(attacker.BattleStats.FrontalAttackModifier);
            return attackModifiers;
        }

        public static void PlayAllyAttackAnimation(int damage = 0)
        {
            Object.FindObjectOfType<AllySpriteController>().StartAttackAnimation();
            if (damage != 0)
            {
                Object.FindObjectOfType<EnemySpriteController>().StartBlinkAnimation();
            }
        }

        public static void PlayEnemyAttackAnimation(int damage = 0)
        {
            Object.FindObjectOfType<EnemySpriteController>().StartAttackAnimation();
            if (damage != 0)
            {
                Object.FindObjectOfType<AllySpriteController>().StartBlinkAnimation();
            }
        }

        private void SingleAttack(Unit attacker, Unit defender)
        {
            var attackModifier = GetAttackModifiers();

            int damage = defender.InflictDamage(attacker.BattleStats.GetDamage(attackModifier), attacker);
            if (attacker.Player.IsPlayerControlled)
            {
                PlayAllyAttackAnimation(damage);
                OnEnemyTakesDamage(damage);
            }

            if (defender.Player.IsPlayerControlled)
            {
                PlayEnemyAttackAnimation(damage);
                OnAllyTakesDamage(damage);
            }
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(ATTACK_DELAY);
            SingleAttack(attacker, defender);
            if (!defender.IsAlive())
            {
                EndFight();
                yield break;
            }

            yield return new WaitForSeconds(1.5f);
            if (attackCount == 0)
                EndFight();
        }

        private IEnumerator End()
        {
            yield return new WaitForSeconds(1.0f);
            if (!attacker.IsAlive())
            {
                attacker.Die();
            }

            if (!defender.IsAlive())
            {
                defender.Die();
            }

            UnitActionSystem.OnCommandFinished -= EndFight;
            UiSystem.OnContinuePressed = null;
            MainScript.Instance.GameStateManager.SwitchState(GameStateManager.GameplayState);
            attacker.UnitTurnState.UnitTurnFinished();
            UnitActionSystem.OnCommandFinished();
        }

        private void EndFight()
        {
            MainScript.Instance.StartCoroutine(End());
        }
    }
}