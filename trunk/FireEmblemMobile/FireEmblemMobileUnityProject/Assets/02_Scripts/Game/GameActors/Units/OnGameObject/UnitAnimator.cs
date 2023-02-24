using System;
using System.Collections.Generic;
using Game.Mechanics;
using LostGrace;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitAnimator : MonoBehaviour
    {

        public event Action OnAnimationEnded;
        public event Action OnAttackAnimationConnected;

        [SerializeField] private Animator animator;
        [SerializeField] private AnimationSpriteSwapper spriteSwapper;
        private static readonly int Selected = Animator.StringToHash("Selected");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int AnimationUp = Animator.StringToHash("BattleAnimationUp");
        private static readonly int AnimationDown = Animator.StringToHash("BattleAnimationDown");
        private static readonly int AnimationLeft = Animator.StringToHash("BattleAnimationLeft");
        private static readonly int AnimationRight = Animator.StringToHash("BattleAnimationRight");
        private static readonly int AnimationDownLeft = Animator.StringToHash("BattleAnimationDownLeft");
        private static readonly int AnimationDownRight = Animator.StringToHash("BattleAnimationDownRight");
        private static readonly int AnimationUpLeft = Animator.StringToHash("BattleAnimationUpLeft");
        private static readonly int AnimationUpRight = Animator.StringToHash("BattleAnimationUpRight");
        [FormerlySerializedAs("unitBp")] public Unit unit;
        private Vector3 lastPosition;
        private bool moving;
        void Start()
        {
            // if(unit!=null)
            //  unit.TurnStateManager.onSelected += SetSelected;
            lastPosition = transform.position;
        }

        private void Update()
        {
            // Debug.Log(name+" "+Vector3.Distance(lastPosition,transform.position));
            if (Vector3.Distance(lastPosition,transform.position)>0.01f)
            {
               
                if (!moving)
                {
                    //Debug.Log("Move!");
                    
                    moving = true;
                    animator.SetBool(Moving, moving);
                }
            }
            else if(moving)
            {
                moving = false;
               // Debug.Log("Stop Move!");
                animator.SetBool(Moving, moving);
            }
            lastPosition = transform.position;
        }

        private void OnDestroy()
        {
            // if(unit!=null)
            //     unit.TurnStateManager.onSelected -= SetSelected;
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

        // public void SetSelected(bool selected)
        // {
        //     animator.SetBool(Selected, selected);
        // }
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
        // protected AnimationClipOverrides clipOverrides;
        public void SetUnit(Unit unit1)
        {
            this.unit = unit1;
            Debug.Log("INIT SPRITE SWAPPER");
            spriteSwapper.Init(unit.Visuals.CharacterSpriteSet);
            // if (unit1.visuals.CharacterSpriteSet.idleAnimation != null)
            //     return;
            // AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            //
            //
            //
            // animator.runtimeAnimatorController = aoc;
            // clipOverrides = new AnimationClipOverrides(aoc.overridesCount);
            // aoc.GetOverrides(clipOverrides);
            //
            // if (unit1.visuals.CharacterSpriteSet.idleAnimation != null)
            // {
            //     clipOverrides["Idle"] = unit1.visuals.CharacterSpriteSet.idleAnimation;
            // }
            // aoc.ApplyOverrides(clipOverrides);
        }
    }

    // public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    // {
    //     public AnimationClipOverrides(int capacity) : base(capacity) {}
    //
    //     public AnimationClip this[string name]
    //     {
    //         get { return this.Find(x => x.Key.name.Equals(name)).Value; }
    //         set
    //         {
    //             int index = this.FindIndex(x => x.Key.name.Equals(name));
    //             if (index != -1)
    //                 this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
    //         }
    //     }
    // }
}