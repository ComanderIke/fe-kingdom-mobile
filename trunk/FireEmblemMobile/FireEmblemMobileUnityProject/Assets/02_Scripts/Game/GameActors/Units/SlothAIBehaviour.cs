using System;
using System.Linq;
using Codice.Client.BaseCommands.Revert;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.EffectMixins;
using Game.Manager;
using Game.States;
using Game.States.Mechanics.Commands;
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
        [SerializeField] private ApplyBuffSkillEffectMixin applysleepMixin;
        [SerializeField] private int healSkillIndex;
        [SerializeField] private int maxSkillUses = 1;
        private int healSkillUses = 1;
        [SerializeField] private float UseHealSkillOnHp = .5f;
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
                    SetSleepMeter(fullSleepMeter);
                    OnSleepMeterChanged?.Invoke();
                    MonoUtility.DelayFunction(()=> AnimationQueue.OnAnimationEnded?.Invoke(),sleepPointDelay);
                });
            }
               
            
        }

        void NoiseMade()
        {
            AnimationQueue.Add(() =>
            {
               SetSleepMeter(sleepMeter+1);
              
                MonoUtility.DelayFunction(()=> AnimationQueue.OnAnimationEnded?.Invoke(),sleepPointDelay);
            });
        }

        void SetSleepMeter(int value)
        {
            sleepMeter=value;
            if (sleepMeter >= fullSleepMeter)
            {
                sleepMeter = fullSleepMeter;
                agent.StatusEffectManager.RemoveDebuff(DebuffType.Slept);
            }
                
            OnSleepMeterChanged?.Invoke();
        }

        public override void Init(Unit agent)
        {
            healSkillUses = maxSkillUses;
            SetSleepMeter(0);
            applysleepMixin = Instantiate(applysleepMixin);
            AfterBattleTasks.OnFinished -= NoiseMade;
            AfterBattleTasks.OnFinished += NoiseMade;
            // UseSkillCommand.OnUseSkill += NoiseMade;
            // UseSkillCommand.OnUseSkill -= NoiseMade;
            // AttackCommand.OnAttackCommandPerformed += NoiseMade;
            // AttackCommand.OnAttackCommandPerformed -= NoiseMade;
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitDamaged += UnitDamaged;
            agent.RevivalStonesChanged-=RevivalStonesChanged;
            agent.RevivalStonesChanged+=RevivalStonesChanged;
            StartOfTurnState.OnStartOfTurnEffects -= StartofTurn;
            StartOfTurnState.OnStartOfTurnEffects += StartofTurn;
            UnitPlacementState.OnStartOfMap -= StartOfMap;
            UnitPlacementState.OnStartOfMap += StartOfMap;
            base.Init(agent);
        }

        void RevivalStonesChanged()
        {
            healSkillUses = maxSkillUses;
        }

        void StartOfMap()
        {
            
            if(sleepMeter<GetMaxSleepMeter())
                applysleepMixin.Activate(agent, agent, 0);
            UnitPlacementState.OnStartOfMap -= StartOfMap;
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
                if (unit.StatusEffectManager.Buffs.Any(d => d.BuffData is DebuffData debuffData&& debuffData.debuffType==DebuffType.Slept))
                {
                    SetState(State.Stunned);
                }
                else
                {
                    if (GetState() == State.Stunned)
                        SetState( State.UseSkill);
                }

                switch (GetState())
                {
                    case State.Aggressive:
                        Debug.Log("HÄH");
                        
                        base.UpdateState(agent, hasAttackableTargets, usedSkill);
                        
                        break;
                    case State.Stunned:
                        //check if still stunned
                        //if yes=> do nothing
                        //if no=> change state to aggressive
                        break;
                    case State.UseSkill:
                        // if (usedSkill)
                        // {
                        //     SetState(State.Aggressive);
                        // }

                        break;
                }
            }
        }
        public override bool CanUseSkill()
        {
            return agent.Hp / (float)agent.MaxHp <= UseHealSkillOnHp && healSkillUses > 0;
        }
        public override void UsedSkill(Skill skillToUse)
        {
            healSkillUses--;
        }
        public override Skill GetSkillToUse()
        {
            if (CanUseSkill())
            {
               
                return agent.SkillManager.ActiveSkills[healSkillIndex];
            }
               
            return null;
        }
    }
}