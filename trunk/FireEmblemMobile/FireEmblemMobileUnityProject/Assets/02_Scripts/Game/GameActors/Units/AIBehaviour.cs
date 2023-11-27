using System;
using System.Collections.Generic;
using System.Linq;
using Game.Mechanics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameActors.Units
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/AIBehaviour", fileName = "AIBehaviour")]
    public class AIBehaviour : ScriptableObject
    {
        [SerializeField] private List<Vector2Int> patrolPoints;
        public enum State
        {
            Patrol,
            Stunned,
            Aggressive,
            UseSkill
        }

        private Unit target;
        private Unit agent;
        private State state = State.Patrol;
        private int rageMeter = 0;
        private int activeIndex = 0;
        [SerializeField] private int fullRageAmount = 4;
        public Action OnRageMeterChanged;

        public int GetMaxRageMeter()
        {
            return fullRageAmount;
        }
        public int GetRageMeter()
        {
            return rageMeter;
        }
        public void Init(Unit agent)
        {
            this.agent = agent;
            target = null;
            state = State.Patrol;
            rageMeter = 0;
            activeIndex = 0;
            Unit.OnUnitDamaged -= UnitDamaged;
            Unit.OnUnitDamaged += UnitDamaged;
        }

        void UnitDamaged(Unit unit, int damage, DamageType damageType, bool crit, bool eff)
        {
            if (unit.Equals(agent))
            {
                rageMeter++;
                if (rageMeter > fullRageAmount)
                    rageMeter = fullRageAmount;
                OnRageMeterChanged?.Invoke();
            }
        }
        public State GetState()
        {
            return state;
        }

        public void UpdateState(IAIAgent agent, bool hasAttackableTargets, bool usedSkill = false)
        {
            if (agent is Unit unit)
            {
                //check if stunned change state to stunned
                if (unit.StatusEffectManager.Debuffs.Any(d => d.name.Contains("Stunned")))
                {
                    state = State.Stunned;
                }
                else
                {
                    if (state == State.Stunned)
                        state = State.Aggressive;
                }

                switch (state)
                {
                    case State.Aggressive:
                        if (rageMeter >= fullRageAmount)
                        {
                            state = State.UseSkill;
                        }
                        else
                        {
                            state = State.Aggressive;
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
                            state = State.Aggressive;
                            rageMeter = 0;
                            OnRageMeterChanged?.Invoke();
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
                            state = State.Aggressive;
                            UpdateState(agent, hasAttackableTargets);
                        }
                           
                        //check if enemy is in range
                        //if Yes => change state to aggressive
                        //if No => Move Unit along the patrol points
                        break;
                }
            }
        }

        public List<Vector2Int> GetPatrolPoints()
        {
            return patrolPoints;
        }

       

        public void UpdatePatrolPoint()
        {
            activeIndex++;
            if (activeIndex >= patrolPoints.Count)
                activeIndex = 0;
        }
        public Vector2Int GetActivePatrolPoints()
        {
            return patrolPoints[activeIndex];
        }
    }
}