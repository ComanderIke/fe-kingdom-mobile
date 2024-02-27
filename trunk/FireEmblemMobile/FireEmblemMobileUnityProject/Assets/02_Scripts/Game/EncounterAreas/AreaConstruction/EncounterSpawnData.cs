using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameResources;
using UnityEngine;
[Serializable]
public class ColumnSpawn
{
    public List<EncounterNodeData> fixedNodes;
}

[Serializable]
public class EncounterSpawnData
{
    public float encounter2ChildPercentage=0.4f;
    public float encounter3ChildPercentage=0.45f;
    public float chanceToConnectToOtherNode = 0.5f;
    public int columnMaxEncounter = 3;
  
    public EncounterNodeData startNodeData;
    public List<EncounterNodeData> allNodeDatas;
    public List<EncounterNodeData> nodeDatas;
    public List<EncounterNodeData> battleNodeDatas;
    public EncounterNodeData normalBattleNodeData;
    public float ChanceDistributionAfterOccurence = 0.05f;

    [HideInInspector]
    public float sumAllStartChances;
    [HideInInspector]
    public Dictionary<EncounterNodeData, float> EncounterChances = new Dictionary<EncounterNodeData, float>();
    [HideInInspector]
    public Dictionary<EncounterNodeData, float> BattleEncounterChances = new Dictionary<EncounterNodeData, float>();
    // [HideInInspector]
    // public Dictionary<EncounterNodeData, float> StartEncounterChances = new Dictionary<EncounterNodeData, float>();

   

    public void InitNodeAppearanceChances()
    {
        EncounterChances = new Dictionary<EncounterNodeData, float>();
        BattleEncounterChances = new Dictionary<EncounterNodeData, float>();
        // StartEncounterChances = new Dictionary<EncounterNodeData, float>();
        foreach (var encounterData in nodeDatas)
        {
            // Debug.Log("add: " + encounterData);
            EncounterChances.Add(encounterData, encounterData.appearanceChance);
            // StartEncounterChances.Add(encounterData, encounterData.appearanceChance);
        }
        foreach (var encounterData in battleNodeDatas)
        {
            // Debug.Log("add: " + encounterData);
            if (encounterData is BattleEncounterNodeData battleEncounterNodeData)
            {

                if ((battleEncounterNodeData).battleType == BattleType.Elite)
                    BattleEncounterChances.Add(encounterData,
                        encounterData.appearanceChance * Player.Instance.Modifiers.EliteBattleRate);
                else
                {
                    BattleEncounterChances.Add(encounterData, encounterData.appearanceChance);
                }
               
            }
            else
            {
                BattleEncounterChances.Add(encounterData, encounterData.appearanceChance);
            }
            // StartEncounterChances.Add(encounterData, encounterData.appearanceChance);
        }
     
        // sumAllStartChances = 0;
        // foreach (var keyvaluepair in EncounterChances)
        // {
        //     sumAllStartChances += keyvaluepair.Value;
        // }
        
    }

    public int GetColumnCount(int areaIndex)
    {
        return GameBPData.Instance.AreaDataList[areaIndex].ColumnSpawns.Count + 1;
    }

    public List<ColumnSpawn> GetColumns(int areaIndex)
    {
        return GameBPData.Instance.AreaDataList[areaIndex].ColumnSpawns;

    }
}