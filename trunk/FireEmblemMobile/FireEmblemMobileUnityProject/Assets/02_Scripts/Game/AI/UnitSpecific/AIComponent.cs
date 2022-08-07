using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public class AIComponent
    {
        public AIComponent()
        {
            AIGoals = new List<Goal>();
            Targets = new List<AITarget>();
        }

       

        public List<Goal> AIGoals { get; set; }
        public WeightSet WeightSet { get; set; }
        public List<AITarget> Targets { get; set; }
        public AITarget ClosestTarget { get; set; }

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
            return Targets.Find(target => target.Actor == unit);
        }
    }
}