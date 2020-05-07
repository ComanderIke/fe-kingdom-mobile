using System.Collections.Generic;
using Assets.AI;
using UnityEngine;

namespace Assets.GameActors.Units
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