using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.AI.AttackReactions
{
    public class RevengeStrike : AttackReaction
    {
        public RevengeStrike(LivingObject unit):base(unit)
        {
            Name = "RevengeStrike";
        }

        public override void Execute()
        {
            MainScript.GetInstance().StartCoroutine(Revenge());
            Debug.Log("Revenge Strike!");
        }
        IEnumerator Revenge()
        {
            yield return new WaitForSeconds(0.25f);
            Attack(User, MainScript.GetInstance().gridManager.GetTileFromVector2(TargetPositions[0]).character);
            yield return new WaitForSeconds(3.5f);
            EventContainer.reactionFinished();
        }
        private void Attack(LivingObject attacker, LivingObject defender)
        {
            if (DoesAttackHit(attacker, defender))
            {
                int damage = defender.InflictDamage(attacker.BattleStats.GetDamage(), attacker);
                MainScript.GetInstance().GetController<UIController>().attackUIController.ShowCounterDamageText(damage);
                GameObject.FindObjectOfType<EnemySpriteController>().StartAttackAnimation();
                GameObject.FindObjectOfType<AllySpriteController>().ShakeAnimation(10 + 1f * damage);
                GameObject.FindObjectOfType<AllySpriteController>().StartBlinkAnimation();
            }
            else
            {
                MainScript.GetInstance().GetController<UIController>().attackUIController.ShowCounterMissText();
            }
        }
        private bool DoesAttackHit(LivingObject attacker, LivingObject defender)
        {
            int rng = UnityEngine.Random.Range(1, 101);
            return rng <= (attacker.BattleStats.GetHitAgainstTarget(defender));
        }
    }
}
