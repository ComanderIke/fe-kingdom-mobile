using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class AIAgent
    {
        public AIAgent()
        {
            AIGoals = new List<Goal>();
            WeightSet = ScriptableObject.CreateInstance<WeightSet>();
            //TODO Assigning WeightSets!
        }

        public List<Goal> AIGoals { get; set; }
        public WeightSet WeightSet { get; set; }
    }
}