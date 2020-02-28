
using Assets.Scripts.AI;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent
{
    public List<Goal> AIGoals { get; set; }
    public WeightSet WeightSet { get; set; }

    public AIAgent()
    {
        AIGoals = new List<Goal>();
        WeightSet = new WeightSet();//TODO fix Warning Assets.Scripts.AI.WeightSet must be instantiated using the ScriptableObject.CreateInstance method instead of new WeightSet.
        //Debug.Log("TODO Asigning WeightSets!");
    }
}