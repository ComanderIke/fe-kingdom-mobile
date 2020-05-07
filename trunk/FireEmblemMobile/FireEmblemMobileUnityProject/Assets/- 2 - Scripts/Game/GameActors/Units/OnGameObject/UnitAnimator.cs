
using System;
using UnityEngine;


namespace Assets.GameActors.Units.OnGameObject
{
    public class UnitAnimator : MonoBehaviour
    {

        public event Action OnAnimationEnded;
        public event Action OnAttackAnimationConnected;

        [SerializeField] private Animator animator;
        public void AttackConnected()
        {
            OnAttackAnimationConnected?.Invoke();
            //Debug.Log("Attack Connected!");
        }
        public void AnimationEnded()
        {
            OnAnimationEnded?.Invoke();
            //Debug.Log("Attack Finished!");
        }

        public void BattleAnimationUp()
        {
            animator.SetTrigger("BattleAnimationUp");
        }
        public void BattleAnimationDown()
        {
            animator.SetTrigger("BattleAnimationDown");
        }
        public void BattleAnimationLeft()
        {
            animator.SetTrigger("BattleAnimationLeft");
        }
        public void BattleAnimationRight()
        {
            animator.SetTrigger("BattleAnimationRight");
        }
        public void BattleAnimationDownLeft()
        {
            animator.SetTrigger("BattleAnimationDownLeft");
        }
        public void BattleAnimationDownRight()
        {
            animator.SetTrigger("BattleAnimationDownRight");
        }
        public void BattleAnimationUpLeft()
        {
            animator.SetTrigger("BattleAnimationUpLeft");
        }
        public void BattleAnimationUpRight()
        {
            animator.SetTrigger("BattleAnimationUpRight");
        }
    }
}