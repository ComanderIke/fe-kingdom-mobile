using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameActors.Units
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/AIBehaviour/Base", fileName = "AIBehaviour")]
    public class AIBehaviour : ScriptableObject
    {
        [SerializeField] private List<Vector2Int> patrolPoints;
        public enum State
        {
            Patrol,
            Stunned,
            Aggressive,
            UseSkill,
            OnRange,
            Guard
        }

        protected Unit target;
        protected Unit agent;
        [HideInInspector]
        public AIGroup aiGroup;
        [SerializeField] private State state = State.Patrol;
        private int activeIndex = 0;

      
        
        public virtual void Init(Unit agent)
        {
            this.agent = agent;
            target = null;
            //state = State.Patrol;
            activeIndex = 0;
         
        }

       
        public State GetState()
        {
            if (aiGroup != null)
                return aiGroup.state;
            return state;
        }

        public void SetState(State state)
        {
            if (aiGroup != null)
                aiGroup.SetState(state);
            this.state = state;
        }
        public virtual void UpdateState(IAIAgent agent, bool hasAttackableTargets, bool usedSkill = false)
        {
            if (agent is Unit unit)
            {
                switch (GetState())
                {
                    case State.Patrol:
                        if (hasAttackableTargets)
                        {
                            SetState(State.Aggressive);
                            UpdateState(agent, hasAttackableTargets);
                        }
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

        public bool WillAttackOnTargetInRange()
        {
            return GetState() == AIBehaviour.State.Aggressive||GetState() == AIBehaviour.State.UseSkill || GetState() == State.OnRange||GetState()==State.Guard;
        }

        public bool WillMoveIfAble()
        {
            return GetState() != State.Guard;
        }

        public bool WillStayIfNoEnemies()
        {
            return GetState() == AIBehaviour.State.Guard || GetState() == AIBehaviour.State.OnRange;
        }

        public void AttackTriggered()
        {
            if (GetState() == State.Patrol || GetState() == State.OnRange)
            {
                SetState( State.Aggressive);
            }
        }

        public virtual Skill GetSkillToUse()
        {
            return agent.SkillManager.ActiveSkills.First();
        }

        public virtual bool CanUseSkill()
        {
            return agent.SkillManager.ActiveSkills.Count > 0;
        }

        public virtual void UsedSkill(Skill skillToUse)
        {
            
        }
    }
}