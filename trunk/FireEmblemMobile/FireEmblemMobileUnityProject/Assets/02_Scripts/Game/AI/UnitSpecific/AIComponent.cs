using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Grid;
using UnityEngine;

namespace Game.AI
{
    public class AIComponent
    {
        public AIComponent(AIBehaviour aiBehaviour, Unit unit, bool initBehaviour=true)
        {
            AIGoals = new List<Goal>();
            Targets = new List<AITarget>();
            MovementOptions = new List<Vector2Int>();
            AttackableTargets = new List<AIAttackTarget>();
            AIBehaviour = aiBehaviour;
            if(AIBehaviour!=null&&initBehaviour)
                AIBehaviour.Init(unit);
        }

       public AIBehaviour AIBehaviour { get; set; }

        public List<Vector2Int> MovementOptions{ get; set; }

        public List<AIAttackTarget> AttackableTargets { get; set; }
        public List<Goal> AIGoals { get; set; }
        public WeightSet WeightSet { get; set; }
        public List<AITarget> Targets { get; set; }
        public AITarget ClosestTarget { get; set; }
        public AIAttackTarget BestAttackTarget { get; set; }

        public int DistanceToClosestEnemy()
        {
            if (Targets == null || Targets.Count==0)
                return -1;
            AITarget target = Targets.Aggregate((min, x) => x.Distance < min.Distance ? x : min);
            ClosestTarget = target;
            return target.Distance;
        }

        public AITarget GetTarget(IGridActor unit)
        {
            return Targets.Find(target => target.TargetObject == unit);
        }
    }
}