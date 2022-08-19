using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EncounterSpawnData
{
    public int columnCount;
    public float encounter2ChildPercentage=0.5f;
    public float encounter3ChildPercentage=0.3f;
    
    public float chanceToShareChild = 0.3f;
    
    public int columnMaxEncounter = 4;
    public EncounterNodeData startNodeData;
    public EncounterNodeData FirstEncounter;
    public List<EncounterNodeData> nodeDatas;
    public EncounterNodeData endNodeData;
    public float ChanceDistributionAfterOccurence = 0.05f;

    [HideInInspector]
    public float sumAllStartChances;
    [HideInInspector]
    public Dictionary<EncounterNodeData, float> EncounterChances = new Dictionary<EncounterNodeData, float>();
    [HideInInspector]
    public Dictionary<EncounterNodeData, float> StartEncounterChances = new Dictionary<EncounterNodeData, float>();

   

    public void InitNodeAppearanceChances()
    {
        EncounterChances = new Dictionary<EncounterNodeData, float>();
        StartEncounterChances = new Dictionary<EncounterNodeData, float>();
        foreach (var encounterData in nodeDatas)
        {
            // Debug.Log("add: " + encounterData);
            EncounterChances.Add(encounterData, encounterData.appearanceChance);
            StartEncounterChances.Add(encounterData, encounterData.appearanceChance);
        }
     
        sumAllStartChances = 0;
        foreach (var keyvaluepair in EncounterChances)
        {
            sumAllStartChances += keyvaluepair.Value;
        }
        
    }

}