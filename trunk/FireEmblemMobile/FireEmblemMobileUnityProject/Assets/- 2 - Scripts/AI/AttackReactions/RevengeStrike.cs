using Assets.Scripts.Characters;
using UnityEngine;
using System.Collections;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.AI.AttackReactions
{
    public class RevengeStrike : AttackReaction
    {
        public RevengeStrike(Unit unit):base(unit)
        {
            Name = "RevengeStrike";
        }

        public override void Execute()
        {
            MainScript.instance.StartCoroutine(Revenge());
            Debug.Log("Revenge Strike!");
        }
        IEnumerator Revenge()
        {
            yield return new WaitForSeconds(0.25f);
            Attack(User, MainScript.instance.GetSystem<global::MapSystem>().GetTileFromVector2(TargetPositions[0]).character);
            yield return new WaitForSeconds(3.5f);
            UnitActionSystem.onReactionFinished();
        }
        private void Attack(Unit attacker, Unit defender)
        {
            if (BattleSystem.DoesAttackHit(attacker, defender))
            {
                int damage = defender.InflictDamage(attacker.BattleStats.GetDamage(), attacker);
                BattleSystem.onAllyTakesDamage(damage);
                BattleSystem.PlayEnemyAttackAnimation(damage);
            }
            else
            {
                BattleSystem.PlayEnemyAttackAnimation();
                BattleSystem.onEnemyMisses();
            }
        }
        private bool DoesAttackHit(Unit attacker, Unit defender)
        {
            int rng = UnityEngine.Random.Range(1, 101);
            return rng <= (attacker.BattleStats.GetHitAgainstTarget(defender));
        }
    }
}
