using System;
using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.Graphics.BattleAnimations;
using Game.GUI.Utility;
using Game.Manager;
using Game.Systems;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitAnimator : MonoBehaviour
    {

        public event Action OnAnimationEnded;
        public event Action OnAttackAnimationConnected;

        public bool setStunnedOnBeginning = false;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer weaponSpriteRenderer;
        [SerializeField] private SpriteRenderer specialSpriteRenderer;
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
        [SerializeField] private MMF_Player walkInFeedbacks;
        [SerializeField] private MMF_Player idleFeedbacks;
        [SerializeField] private MMF_Player criticalFeedbacks;
        [SerializeField] private MMF_Player attackFeedbacks;
        [SerializeField] private MMF_Player dodgeFeedbacks;
        [SerializeField] private HitFeedbackController hitFeedbackController;
        [FormerlySerializedAs("unitBp")] public Unit unit;
        private Vector3 lastPosition;
        private bool moving;
        private int FrameCountMovingCheck= 0;
        private int frameCount = 0;
        [SerializeField]private MMF_Player deathFeedbacks;
        private static readonly int AlternateWalk = Animator.StringToHash("AlternateWalk");

        void Start()
        {
            // if(unit!=null)
            //  unit.TurnStateManager.onSelected += SetSelected;
            lastPosition = transform.position;
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitDamaged += UnitDamaged;
            if(setStunnedOnBeginning)
                animator.SetBool(Stunned,true);
                
        }

        
        private void Update()
        {
            if (lockAnimation)
                return;
           // Debug.Log(name+" "+Vector3.Distance(lastPosition,transform.position));
           

           if (spriteRenderer != null)
           {
               float xDiff = (lastPosition.x - transform.position.x);
               if (xDiff > 0.003f)
               {
                   spriteRenderer.flipX = true;
               }
               else if(xDiff<-0.003f)
                   spriteRenderer.flipX = false;
               else
               {
                   // spriteRenderer.flipX = !GridGameManager.Instance.FactionManager.IsActiveFaction(unit.Faction);
               }
               var flipX = spriteRenderer.flipX;
               if (weaponSpriteRenderer != null)
                   weaponSpriteRenderer.flipX = flipX;
               if (specialSpriteRenderer != null)
                   specialSpriteRenderer.flipX = flipX;
           }

           if (Vector3.Distance(lastPosition,transform.position)>0.001f)
            {
                frameCount=0;
                if (!moving)
                {
                   // Debug.Log("Move!"+name);
                    
                    moving = true;
                    animator.SetBool(Moving, moving);
                }
            }
            else if(moving)
            {
                frameCount++;
                if (frameCount >= FrameCountMovingCheck)
                {
                   moving = false;
                    //Debug.Log("Stop Move!");
                    animator.SetBool(Moving, moving);
                }
            }
            lastPosition = transform.position;
        }

      

        private void OnDisable()
        {
            if (unit != null)
            {
                Unit.OnUnitDamaged -= UnitDamaged;
                if (unit.StatusEffectManager != null)
                {
                    unit.StatusEffectManager.OnStatusEffectAdded -= StatusEffectAnimation;
                    unit.StatusEffectManager.OnStatusEffectRemoved -= StatusEffectRemovedAnimation;
                }

                unit.OnSpecialState -= SpecialState;
            }
            //     unit.TurnStateManager.onSelected -= SetSelected;
        }

        private void UnitDamaged(Unit unit1, int damage, DamageType damagetype, bool crit, bool eff)
        {
            if (unit1 == unit)
            {
                DamagedState state = DamagedState.Damage;
                if (crit)
                    state = DamagedState.HighDmg;
                if (damage == 0)
                    state = DamagedState.NoDamage;
                Damaged(1.0f, state);

            }
                
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
        public void Attack(float playSpeed)
        {
            // PlayAtSpeed(attack, playSpeed);
            attackFeedbacks.PlayFeedbacks();
          //  spriteRenderer.sortingOrder = sortOrderAttack;
      
        }
        public void Critical(float playSpeed)
        {
            //PlayAtSpeed(critical, playSpeed);
            criticalFeedbacks.PlayFeedbacks();
           // spriteRenderer.sortingOrder = sortOrderAttack;
        }
        public void Death(float playSpeed)
        {
            // PlayAtSpeed(death, playSpeed);
       
            deathFeedbacks.PlayFeedbacks();
           // spriteRenderer.sortingOrder = sortOrderNormal;
        }
        public void Dodge(float playSpeed)
        {
            //PlayAtSpeed(dodge, playSpeed);
            dodgeFeedbacks.PlayFeedbacks();
           // spriteRenderer.sortingOrder = sortOrderNormal;
        }

    
        public void Damaged(float playSpeed,DamagedState damagedState)
        {

            if (hitFeedbackController == null)
                return;
            hitFeedbackController.SetState(damagedState);
            hitFeedbackController.PlayHitFeedback();
         //  spriteRenderer.sortingOrder = sortOrderNormal;
            // PlayAtSpeed(damaged, playSpeed);
        }
        public void DeathAnimation()
        {
            deathFeedbacks.PlayFeedbacks();
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
        // protected AnimationClipOverrides clipOverrides;
        void StatusEffectAnimation(Unit u, BuffDebuffBase effect)
        {
            Debug.Log("StatusEffectAdded: "+effect.name);
            if (effect.name.Contains("Stunned"))
            {
                animator.SetBool(Stunned,true);
            }

            if (effect.BuffData is DebuffData debuffData)
            {
                if (debuffData.debuffType == DebuffType.Slept)
                {
                    animator.SetBool(Stunned,true);
                }
            }
        }
        void StatusEffectRemovedAnimation(Unit u, BuffDebuffBase effect)
        {
            if (effect.name.Contains("Stunned"))
            {
                animator.SetBool(Stunned,false);
            }
            
            if (effect.BuffData is DebuffData debuffData)
            {
                if (debuffData.debuffType == DebuffType.Slept)
                {
                    animator.SetBool(Stunned,false);
                }
            }
        }

        void SpecialState(bool value)
        {
            spriteSwapper.SetSpecialState(value);
        }
        public void SetUnit(Unit unit1)
        {
            this.unit = unit1;
            unit.StatusEffectManager.OnStatusEffectAdded += StatusEffectAnimation;
            unit.StatusEffectManager.OnStatusEffectRemoved+= StatusEffectRemovedAnimation;
            unit.OnSpecialState += SpecialState;
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

        private bool lockAnimation = false;
        private static readonly int Stunned = Animator.StringToHash("Stunned");

        public void SetAlternateWalk(bool value)
        {
            lockAnimation = value;
            animator.SetBool("AlternateWalk", value);
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