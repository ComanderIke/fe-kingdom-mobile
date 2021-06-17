using System;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitAnimator : MonoBehaviour
    {

        public event Action OnAnimationEnded;
        public event Action OnAttackAnimationConnected;

        [SerializeField] private Animator animator;
        private static readonly int Selected = Animator.StringToHash("Selected");
        private static readonly int AnimationUp = Animator.StringToHash("BattleAnimationUp");
        private static readonly int AnimationDown = Animator.StringToHash("BattleAnimationDown");
        private static readonly int AnimationLeft = Animator.StringToHash("BattleAnimationLeft");
        private static readonly int AnimationRight = Animator.StringToHash("BattleAnimationRight");
        private static readonly int AnimationDownLeft = Animator.StringToHash("BattleAnimationDownLeft");
        private static readonly int AnimationDownRight = Animator.StringToHash("BattleAnimationDownRight");
        private static readonly int AnimationUpLeft = Animator.StringToHash("BattleAnimationUpLeft");
        private static readonly int AnimationUpRight = Animator.StringToHash("BattleAnimationUpRight");
        public Unit unit;
        void Start()
        {
            if(unit!=null)
             unit.TurnStateManager.onSelected += SetSelected;
        }

        private void OnDestroy()
        {
            if(unit!=null)
                unit.TurnStateManager.onSelected -= SetSelected;
        }

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

        public void SetSelected(bool selected)
        {
            animator.SetBool(Selected, selected);
        }
        public void BattleAnimationUp()
        {
            animator.SetTrigger(AnimationUp);
        }
        public void BattleAnimationDown()
        {
            animator.SetTrigger(AnimationDown);
        }
        public void BattleAnimationLeft()
        {
            animator.SetTrigger(AnimationLeft);
        }
        public void BattleAnimationRight()
        {
            animator.SetTrigger(AnimationRight);
        }
        public void BattleAnimationDownLeft()
        {
            animator.SetTrigger(AnimationDownLeft);
        }
        public void BattleAnimationDownRight()
        {
            animator.SetTrigger(AnimationDownRight);
        }
        public void BattleAnimationUpLeft()
        {
            animator.SetTrigger(AnimationUpLeft);
        }
        public void BattleAnimationUpRight()
        {
            animator.SetTrigger(AnimationUpRight);
        }
    }
}