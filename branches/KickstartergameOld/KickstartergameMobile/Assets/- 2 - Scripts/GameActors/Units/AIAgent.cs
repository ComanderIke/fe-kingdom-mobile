
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
        WeightSet = new WeightSet();
        Debug.Log("TODO Asigning WeightSets!");
    }
}