using System;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.Systems;
using Game.Utility;
using UnityEngine;
using Utility;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/AIBehaviour/Minotaur", fileName = "AIBehaviourMinotaur")]
    public class MinotaurAIBehaviour : BossAIBehaviour
    {
        private int rageMeter = 0;
        [SerializeField] private int fullRageAmount = 4;
        public int fullRageSkillIndex = 1;
       
        public Action OnRageMeterChanged;
        public int GetMaxRageMeter()
        {
            return fullRageAmount;
        }
        public int GetRageMeter()
        {
            return rageMeter;
        }

        private float ragePointDelay = 0.8f;
        void UnitDamaged(Unit unit, int damage, DamageType damageType, bool crit, bool eff)
        {
            if (unit.Equals(agent))
            {
                AnimationQueue.Add(() =>
                {
                    rageMeter++;
                    SetState(State.UseSkill);
                    if (rageMeter > fullRageAmount)
                        rageMeter = fullRageAmount;
                    OnRageMeterChanged?.Invoke();
                    MonoUtility.DelayFunction(()=> AnimationQueue.OnAnimationEnded?.Invoke(),ragePointDelay);
                });
            };
               
            
        }

        public override void UsedSkill(Skill skillToUse)
        {
            rageMeter = 0;
            OnRageMeterChanged?.Invoke();
            base.UsedSkill(skillToUse);
        }

        public override Skill GetSkillToUse()
        {
            if (rageMeter >= fullRageAmount)
                return agent.SkillManager.ActiveSkills[fullRageSkillIndex];
            return null;
        }

        public override void Init(Unit agent)
        {
            rageMeter = 0;
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitDamaged += UnitDamaged;
            base.Init(agent);
        }
        public override void UpdateState(IAIAgent agent, bool hasAttackableTargets)
        {
            if (agent is Unit unit)
            {
                //check if stunned change state to stunned
                if (unit.StatusEffectManager.Buffs.Any(d => d.BuffData is DebuffData debuffData&& debuffData.debuffType==DebuffType.Stunned))
                {
                    SetState(State.Stunned);
                }
                else
                {
                    if (GetState() == State.Stunned)
                        SetState( State.Aggressive);
                }
                if (rageMeter >= fullRageAmount)
                {
                    SetState(State.UseSkill);
                }

                switch (GetState())
                {
                    case State.Aggressive:
                        Debug.Log("HÄH");
                        
                            base.UpdateState(agent, hasAttackableTargets);
                        break;
                    case State.Stunned:
                        //check if still stunned
                        //if yes=> do nothing
                        //if no=> change state to aggressive
                        break;
                    case State.UseSkill:
                            
                        // if no enemies in attackrange AND no enemies in Range to stun
                        // do normal aggressive behaviour
                        // EITHER
                        // otherwise check for best skillTarget on each position
                        // number of enemy targets=> possible kills => most damage done
                        // OR
                        // last enemy who attacked this unit is marked
                        // unit will try to hit this unit with the skill.
                        // still looks for best targets that include this unit
                        // move as close to the target as possible while still able to hit it with the skill
                        // 
                        //After: change state to aggressive or stunned depending on collision
                        if (target == null)
                        {

                        }
                        else
                        {

                        }

                        break;
                    case State.Patrol:
                        if (hasAttackableTargets)
                        {
                            SetState( State.Aggressive);
                            UpdateState(agent, hasAttackableTargets);
                        }
                           
                        //check if enemy is in range
                        //if Yes => change state to aggressive
                        //if No => Move Unit along the patrol points
                        break;
                }
            }
        }
    }
}