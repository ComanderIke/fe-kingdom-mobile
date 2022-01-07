﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EncounterSpawnData
{
    public int columnCount;
    public float encounter2ChildPercentage=0.5f;
    public float encounter3ChildPercentage=0.3f;
    
    public float chanceToShareChild = 0.3f;
    
    public int columnMaxEncounter = 4;
    public EncounterNodeData startNodeData;
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
[Serializable]
public class EncounterTree
{
    private static EncounterTree _instance;
    public EncounterNode startNode;
    public EncounterNode endNode;
    public List<Column> columns;
    public EncounterSpawnData spawnData;
   
    public static EncounterTree Instance
    {
        get { return _instance ??= new EncounterTree(); }
    }

    public EncounterTree()
    {
        columns = new List<Column>();
    }
    bool ShareChild(EncounterNode parent, Column current, Column previous)
    {
        bool bindChild = false;
        float chance = spawnData.chanceToShareChild;
        if (current.children.Count > 0)
        {
            float rng2 = Random.value;
            if (previous.children.Count >= 3)
            {
                chance *= 2;
            }
            if (rng2 <= chance|| current.children.Count + 1 > spawnData.columnMaxEncounter)
            {
                for (int i = current.children.Count-1; i >=0; i--)
                {
                    if (!current.children[i].parents.Contains(parent))
                    {
                        current.children[i].parents.Add(parent);
                        parent.children.Add(current.children[i]);
                        bindChild = true;
                        //Debug.Log("Bind Existing Node!");
                        break;
                    }
                }
            }
        }
        return bindChild;
    }
    void SpawnSingleEncounter(EncounterNode parent, Column current,Column previous)
    {
        if (!ShareChild(parent, current, previous))
        {
            
            float threshold = 0;
            EncounterNodeData chosenKey =null;
            float sumAllChances = 0;
            foreach (var key in spawnData.EncounterChances.Keys)
            {
                sumAllChances += spawnData.EncounterChances[key];
            }

            float rng = Random.Range(0, sumAllChances);
            foreach (var key in spawnData.EncounterChances.Keys)
            {
                threshold +=spawnData.EncounterChances[key];
                if (rng <= threshold)
                {
                    UpdateEncounterChances(key);
                    chosenKey = key;
                    break;
                }
                    
            }
            Debug.Log(chosenKey);
            EncounterNode node = chosenKey.CreateNode(parent);
            node.prefabIdx = GetNodeDataIndex(chosenKey);
            //CreateNodeGameObject(chosenKey.prefab, node);
           
            current.children.Add((node));
            parent.children.Add(node);
            //Debug.Log("Spawn New Node!");
        }
    }
    private int GetNodeDataIndex(EncounterNodeData data)
    {
        return spawnData.nodeDatas.IndexOf(data);
    }
    private void UpdateEncounterChances(EncounterNodeData paramkey)
    {
        List<EncounterNodeData> keys = new List<EncounterNodeData>(spawnData.EncounterChances.Keys);
        foreach (var key in keys)
        {
            if (key==paramkey)
            {
                spawnData.EncounterChances[key] = spawnData.StartEncounterChances[key]-spawnData.ChanceDistributionAfterOccurence;
                if (spawnData.EncounterChances[key] < 0)
                    spawnData.EncounterChances[key] = 0;
            }
            else
            {
                spawnData.EncounterChances[key] += spawnData.ChanceDistributionAfterOccurence*(spawnData.StartEncounterChances[key]/spawnData.sumAllStartChances);
            }
        }
    }
    public void CreateMiddleColumns(List<EncounterNodeData> data)
    {
        for (int i = 1; i < spawnData.columnCount; i++)
        {
            Column column = new Column();
            column.index = i;
            //Debug.Log("Create Column!");
            //Debug.Log(columns.Last().children.Count);
            foreach (EncounterNode node in columns.Last().children)
            {
                //Debug.Log("Spawn Encounter!"+ node+" "+column);
                SpawnEncounters(node, column, columns[column.index-1]);
            }
            
            columns.Add(column);
        }
    }
    void SpawnEncounters(EncounterNode parent, Column current, Column previous)
    {
        float rng = Random.value;
        if (previous.children.Count == 1)
        {
            rng = Random.Range(0, spawnData.encounter2ChildPercentage);
        }
        if (previous.children.Count >= 4)
        {
            rng += spawnData.encounter3ChildPercentage;
        }
        if (rng <= spawnData.encounter3ChildPercentage)
        {
            //Debug.Log("Spawn Triple Node");
            SpawnSingleEncounter(parent, current, previous);
            SpawnSingleEncounter(parent, current, previous);
            SpawnSingleEncounter(parent, current, previous);
        }
        else if (rng <= spawnData.encounter2ChildPercentage)
        {
            
            //Debug.Log("Spawn Double Node");
            SpawnSingleEncounter(parent, current, previous);
            SpawnSingleEncounter(parent, current, previous);

        }
        else
        {
            //Debug.Log("Spawn Single Node");
            SpawnSingleEncounter(parent, current, previous);
        }
    }
    public void CreateStartColumn(EncounterNodeData startNodeData)
    {
        Column startColumn = new Column();
        startNode = startNodeData.CreateNode(null);
        startColumn.children.Add( startNode);
        startColumn.index = 0;
        columns.Add(startColumn);
        
    }

    public void CreateEndColumn(EncounterNodeData endNodeData)
    {
        Column endColumn = new Column();
        if (endNodeData is BattleEncounterNodeData data)
        {
            endNode = new BattleEncounterNode(data.levelIndex, data.EnemyArmyData,null);
        }

        endColumn.children.Add(endNode);
        endColumn.index = spawnData.columnCount;

        columns.Add(endColumn);
    }

    public void Create()
    {
        CreateStartColumn(spawnData.startNodeData);
        CreateMiddleColumns(spawnData.nodeDatas);
        CreateEndColumn(spawnData.endNodeData);
    }
}