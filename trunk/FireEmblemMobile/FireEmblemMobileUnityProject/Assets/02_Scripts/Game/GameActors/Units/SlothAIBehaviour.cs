using System;
using System.Linq;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.EffectMixins;
using Game.Manager;
using Game.States;
using Game.Systems;
using Game.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/AIBehaviour/Sloth", fileName = "AIBehaviourSloth")]
    public class SlothAIBehaviour : BossAIBehaviour
    {
        private int sleepMeter = 0;
        [FormerlySerializedAs("fullRageAmount")] [SerializeField] private int fullSleepMeter = 4;
        [SerializeField]private ApplyBuffSkillEffectMixin applysleepMixin;
        public Action OnSleepMeterChanged;
        public int GetMaxSleepMeter()
        {
            return fullSleepMeter;
        }
        public int GetSleepMeter()
        {
            return sleepMeter;
        }

        private float sleepPointDelay = 0.8f;
        void UnitDamaged(Unit unit, int damage, DamageType damageType, bool crit, bool eff)
        {
            if (unit.Equals(agent))
            {
                AnimationQueue.Add(() =>
                {
                    sleepMeter++;
                    if (sleepMeter > fullSleepMeter)
                        sleepMeter = fullSleepMeter;
                    OnSleepMeterChanged?.Invoke();
                    MonoUtility.DelayFunction(()=> AnimationQueue.OnAnimationEnded?.Invoke(),sleepPointDelay);
                });
            };
               
            
        }

        public override void Init(Unit agent)
        {
            sleepMeter = 0;
            applysleepMixin = Instantiate(applysleepMixin);
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitDamaged += UnitDamaged;
            StartOfTurnState.OnStartOfTurnEffects -= StartofTurn;
            StartOfTurnState.OnStartOfTurnEffects += StartofTurn;
            base.Init(agent);
        }

        void StartofTurn()
        {
            if (GridGameManager.Instance.FactionManager.ActiveFaction.Id != agent.Faction.Id)
                return;
            if(sleepMeter<GetMaxSleepMeter())
                applysleepMixin.Activate(agent, agent, 0);
            else
            {
                StartOfTurnState.OnStartOfTurnEffects -= StartofTurn;
            }
        }
        public override void UpdateState(IAIAgent agent, bool hasAttackableTargets, bool usedSkill = false)
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

                switch (GetState())
                {
                    case State.Aggressive:
                        Debug.Log("HÄH");
                        if (sleepMeter >= fullSleepMeter)
                        {
                            SetState(State.UseSkill);
                        }
                        else
                        {
                            base.UpdateState(agent, hasAttackableTargets, usedSkill);
                            
                        }
                        break;
                    case State.Stunned:
                        //check if still stunned
                        //if yes=> do nothing
                        //if no=> change state to aggressive
                        break;
                    case State.UseSkill:
                        if (usedSkill)
                        {
                            SetState(State.Aggressive);
                            sleepMeter = 0;
                            OnSleepMeterChanged?.Invoke();
                        }
                            
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