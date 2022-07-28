using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AIComponent
    {
        public AIComponent()
        {
            AIGoals = new List<Goal>();
        }

        public List<Goal> AIGoals { get; set; }
        public WeightSet WeightSet { get; set; }
    }
}