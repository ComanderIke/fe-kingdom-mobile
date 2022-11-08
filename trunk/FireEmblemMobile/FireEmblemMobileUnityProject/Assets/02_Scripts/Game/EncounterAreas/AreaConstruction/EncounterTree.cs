﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EncounterTree
{
    private static EncounterTree _instance;
    public EncounterNode startNode;
    public EncounterNode endNode;
    public List<Column> columns;
    public EncounterSpawnData spawnData;
    public List<FixedColumnNode> fixedColumns;

    public Dictionary<EncounterNodeData, int> EncounterAppearanceCount = new Dictionary<EncounterNodeData, int>();
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
        if (current.index <= 2)// dont share a child on first 2 columns
            return false;
        if (current.children.Count > 0)
        {
            float rng2 = Random.value;
            if (previous.children.Count >= 3)
            {
                chance *= 2;
            }
            if (rng2 <= chance|| current.children.Count>= spawnData.columnMaxEncounter)
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
            if (fixedEncounters)
            {
                for (int j = 0; j < fixedColumns.Count; j++)
                {
                    if (fixedColumns[j].columnIndex == current.index)
                    {
                        chosenKey=fixedColumns[j].nodeData;
                       
                    }
                }
            }
            float sumAllChances = 0;
            foreach (var key in spawnData.EncounterChances.Keys)
            {
                sumAllChances += spawnData.EncounterChances[key];
            }

            while (chosenKey == null)
            {
                if (current.index == 1)
                {
                    chosenKey = spawnData.FirstEncounter;
                }
                else
                {
                    float rng = Random.Range(0, sumAllChances);
                    foreach (var key in spawnData.EncounterChances.Keys)
                    {
                        threshold += spawnData.EncounterChances[key];
                        int indexOfKey = spawnData.nodeDatas.IndexOf(key);
                        if (rng <= threshold && (!EncounterAppearanceCount.ContainsKey(key) ||
                                                 EncounterAppearanceCount[key] < spawnData.nodeDatas[indexOfKey]
                                                     .maxAppearanceCountPerArea))
                        {
                            UpdateEncounterChances(key);
                            chosenKey = key;
                            if (EncounterAppearanceCount.ContainsKey(key))
                                EncounterAppearanceCount[key]++;
                            else
                            {
                                EncounterAppearanceCount.Add(key, 1);
                            }

                            break;
                        }

                    }
                }
            }

            //Debug.Log(chosenKey);
            EncounterNode node = chosenKey.CreateNode(parent, current.index,current.children.Count);
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
    public void CreateMiddleColumns()
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
                    SpawnEncounters(node, column, columns[column.index - 1]);
           
            }
            
            columns.Add(column);
        }
    }
    public void CreateMiddleColumns(EncounterTreeData encounterTreeData)
    {
        
        for (int i = 1; i < encounterTreeData.columns.Count-1; i++)
        {
            Column column = new Column();
            Column previous = columns.Last();
            EncounterNode parent = null;
            column.index = i;
            Debug.Log("Create Column!"+i);
            //Debug.Log(columns.Last().children.Count);
            
            for (int j=0; j< encounterTreeData.columns[i].nodeDatas.Count; j++)
            {
                var nodeSpawnData = spawnData.nodeDatas[encounterTreeData.columns[i].nodeDatas[j].nodeTypeIndex];
                EncounterNode node = nodeSpawnData.CreateNode(parent, i,j);
                node.prefabIdx = encounterTreeData.columns[i].nodeDatas[j].nodeTypeIndex;
                column.children.Add(node);
                foreach (var parentIndex in encounterTreeData.columns[i].nodeDatas[j].parentIndexes)
                {
                    node.parents.Add(previous.children[parentIndex]);//Adding parent to child
                    previous.children[parentIndex].children.Add(node);//Adding child to parent
                }
                
            }

            columns.Add(column);
        }
    }
    void SpawnEncounters(EncounterNode parent, Column current, Column previous, EncounterNodeData fixedData=null)
    {
        if (current.children.Count >= spawnData.columnMaxEncounter)
            return;
        if (current.index == 1) //First Column
        {
            SpawnSingleEncounter(parent, current, previous);
            SpawnSingleEncounter(parent, current, previous);
            SpawnSingleEncounter(parent, current, previous);
        }
        else if (current.index == 2) //Second Column
        {
            SpawnSingleEncounter(parent, current, previous);
          
        }
        else
        {
            float rng = Random.value;
         
            /*if (current.children.Count<= spawnData.columnMaxEncounter-2)
            {
                rng += spawnData.encounter2ChildPercentage;
            }
            if (current.children.Count<= spawnData.columnMaxEncounter-3)
            {
                rng += spawnData.encounter3ChildPercentage;
            }*/
            if (rng <= spawnData.encounter3ChildPercentage&& current.children.Count <=spawnData.columnMaxEncounter-3)
            {
                //Debug.Log("Spawn Triple Node");
                SpawnSingleEncounter(parent, current, previous);
                SpawnSingleEncounter(parent, current, previous);
                SpawnSingleEncounter(parent, current, previous);
            }
            else if (rng <= spawnData.encounter2ChildPercentage&& current.children.Count <=spawnData.columnMaxEncounter-2)
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
    }
    public void CreateStartColumn(EncounterNodeData startNodeData)
    {
        Column startColumn = new Column();
        startNode = startNodeData.CreateNode(null,0,0);
        startColumn.children.Add( startNode);
        startColumn.index = 0;
        columns.Add(startColumn);
        
    }

    public void CreateEndColumn(EncounterNodeData endNodeData)
    {
        Column endColumn = new Column();
        if (endNodeData is BattleEncounterNodeData data)
        {
            endNode = new BattleEncounterNode(data.levelIndex, data.EnemyArmyData,null,spawnData.columnCount,0, "Boss Battle", "Area Boss Battle", null);
        }

        endColumn.children.Add(endNode);
        endColumn.index = spawnData.columnCount;

        columns.Add(endColumn);
    }

    private bool fixedEncounters;

    public void Create(bool fixedEncounters, List<FixedColumnNode> fixedColumns)
    {
        this.fixedEncounters = fixedEncounters;
        this.fixedColumns = fixedColumns;
        CreateStartColumn(spawnData.startNodeData);
        CreateMiddleColumns();
        CreateEndColumn(spawnData.endNodeData);
    }
    public void CreateFromData(EncounterTreeData encounterTreeData)
    {
        CreateStartColumn(spawnData.startNodeData);
        CreateMiddleColumns(encounterTreeData);
        CreateEndColumn(spawnData.endNodeData);
    }

    public EncounterTreeData GetSaveData()
    {
        return new EncounterTreeData(this);
    }


    public void LoadData(EncounterTreeData encounterTreeData)
    {
        spawnData = spawnData;
        columns.Clear();
        CreateFromData(encounterTreeData);
    }

    public static void Reset()
    {
        _instance = null;
    }

    public EncounterNode GetEncounterNodeById(string id)
    {
        foreach (var column in columns)
        {
            foreach (var child in column.children)
            {
                if (child.GetId() == id)
                {
                    return child;
                }
            }
        }

        return null;
    }
}