using System;
using System.Collections.Generic;
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
        [SerializeField] protected State state = State.Patrol;
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
            return state;
        }

        public virtual void UpdateState(IAIAgent agent, bool hasAttackableTargets, bool usedSkill = false)
        {
            if (agent is Unit unit)
            {
                switch (state)
                {
                    case State.Patrol:
                        if (hasAttackableTargets)
                        {
                            state = State.Aggressive;
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
            return state == AIBehaviour.State.Aggressive || state == State.OnRange||state==State.Guard;
        }

        public bool WillMoveIfAble()
        {
            return state != State.Guard;
        }

        public bool WillStayIfNoEnemies()
        {
            return state == AIBehaviour.State.Guard || state == AIBehaviour.State.OnRange;
        }

        public void AttackTriggered()
        {
            if (state == State.Patrol || state == State.OnRange)
            {
                state = State.Aggressive;
            }
        }
    }
}