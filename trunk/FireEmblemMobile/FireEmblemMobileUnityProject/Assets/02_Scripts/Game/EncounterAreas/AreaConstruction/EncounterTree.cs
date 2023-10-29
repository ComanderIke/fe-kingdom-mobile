using System;
using System.Collections.Generic;
using System.Linq;
using Game.Systems;
using Random = UnityEngine.Random;

[Serializable]
public class EncounterTree
{
    private static EncounterTree _instance;
    public EncounterNode startNode;
    public List<Column> columns;
    public EncounterSpawnData spawnData;

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
        float chance = spawnData.chanceToConnectToOtherNode;
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
    void SpawnSingleEncounter(List<EncounterNode> parents, Column current)
    {
        float threshold = 0;
        EncounterNodeData chosenKey =null;
        float sumAllChances = 0;
        foreach (var key in spawnData.EncounterChances.Keys)
        {
            sumAllChances += spawnData.EncounterChances[key];
        }

        while (chosenKey == null)
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
        CreateNodeFromChosenKey(chosenKey, parents, current);
    }

    void CreateNodeFromChosenKey(EncounterNodeData chosenKey, List<EncounterNode> parents, Column current)
    {
        EncounterNode node = chosenKey.CreateNode(parents, current.index,current.children.Count);
        node.prefabIdx = GetNodeDataIndex(chosenKey);
        //CreateNodeGameObject(chosenKey.prefab, node);
       
        current.children.Add((node));
        if (parents != null)
        {
            foreach (var parent in parents)
                parent.children.Add(node);
        }
    }
    void SpawnSpecificEncounter(EncounterNodeData encounterData, List<EncounterNode> parents, Column current,Column previous)
    {
        EncounterNode node = encounterData.CreateNode(parents, current.index,current.children.Count);
        node.prefabIdx = GetNodeDataIndex(encounterData);

        current.children.Add((node));
        if (parents != null)
        {
            foreach (var parent in parents)
                parent.children.Add(node);
        }

    }
    private int GetNodeDataIndex(EncounterNodeData data)
    {
        return spawnData.allNodeDatas.IndexOf(data);
    }
    private EncounterNodeData GetNodeDataIndex(int index)
    {
        return spawnData.allNodeDatas[index];
    }
    private void UpdateEncounterChances(EncounterNodeData paramkey)
    {
        List<EncounterNodeData> keys = new List<EncounterNodeData>(spawnData.EncounterChances.Keys);
        foreach (var key in keys)
        {
            if (key==paramkey)
            {
                // spawnData.EncounterChances[key] = spawnData.StartEncounterChances[key]-spawnData.ChanceDistributionAfterOccurence;
                if (spawnData.EncounterChances[key] < 0)
                    spawnData.EncounterChances[key] = 0;
            }
            else
            {
                // spawnData.EncounterChances[key] += spawnData.ChanceDistributionAfterOccurence*(spawnData.StartEncounterChances[key]/spawnData.sumAllStartChances);
            }
        }
    }
    public void CreateMiddleColumns()
    {
        for (int i = 1; i < spawnData.ColumnSpawns.Count+1; i++)
        {
            Column column = new Column();
            column.index = i;
            SpawnEncounters(column, columns[column.index - 1]);
            columns.Add(column);
        }
    }
    public void CreateMiddleColumns(EncounterTreeData encounterTreeData)
    {
        
   
        for (int i = 1; i < encounterTreeData.columns.Count; i++)
        {
            Column column = new Column();
            Column previous = columns.Last();
            column.index = i;
            
            //Debug.Log(columns.Last().children.Count);
            
            for (int j=0; j< encounterTreeData.columns[i].nodeDatas.Count; j++)
            {
                
                var nodeSpawnData = GetNodeDataIndex(encounterTreeData.columns[i].nodeDatas[j].nodeTypeIndex);
                EncounterNode node;
                if (encounterTreeData.columns[i].nodeDatas[j].userDataId != String.Empty&&nodeSpawnData is EventEncounterNodeData eventData)
                {
                    node=eventData.CreateNode(encounterTreeData.columns[i].nodeDatas[j].userDataId,null, i,j);
                }
                else
                {
                    node = nodeSpawnData.CreateNode(null, i,j);
                }
               
               
                node.prefabIdx = encounterTreeData.columns[i].nodeDatas[j].nodeTypeIndex;
               
                column.children.Add(node);
                
                foreach (var parentIndex in encounterTreeData.columns[i].nodeDatas[j].parentIndexes)
                {
                    
                    if (parentIndex < previous.children.Count)
                    {
                        node.parents.Add(previous.children[parentIndex]); //Adding parent to child
                        previous.children[parentIndex].children.Add(node); //Adding child to parent
                    }
                   
                }
                
            }
            
            columns.Add(column);
        }
    }
    void SpawnEncounters(Column current, Column previous)
    {
       if (current.children.Count >= spawnData.columnMaxEncounter)
           return;
       
       var fixedNodes = spawnData.ColumnSpawns[current.index - 1].fixedNodes;
       if (fixedNodes.Count!=0)
       {
           for (int j = 0; j < fixedNodes.Count; j++)
           {
               var chosenKey=fixedNodes[j];
               CreateNodeFromChosenKey(chosenKey, null, current);
           }
       }
       else
       {
           float rng = Random.value;

           if (rng <= spawnData.encounter3ChildPercentage)
           {
               SpawnSingleEncounter(null, current);
               SpawnSingleEncounter(null, current);
               SpawnSingleEncounter(null, current);

           }
           else if (rng <= spawnData.encounter2ChildPercentage + spawnData.encounter3ChildPercentage)
           {
               SpawnSingleEncounter(null, current);
               SpawnSingleEncounter(null, current);
           }
           else
           {
               SpawnSingleEncounter(null, current);
           }
       }

    }
    public void CreateStartColumn(EncounterNodeData startNodeData)
    {
        Column startColumn = new Column();
        startNode = startNodeData.CreateNode(null,0,0);
        startColumn.children.Add( startNode);
        startColumn.index = 0;
        columns.Add(startColumn);//ADDING STUFF TO COLUMN CAUSES FREEZE
        
    }

      
    

    void ResetStaticStuff()
    {
        columns = new List<Column>();
    }
    public void Create()
    {

        ResetStaticStuff();
        CreateStartColumn(spawnData.startNodeData);
        CreateMiddleColumns();
    }
    public void CreateFromData(EncounterTreeData encounterTreeData)
    {
        ResetStaticStuff();
        CreateStartColumn(spawnData.startNodeData);
        CreateMiddleColumns(encounterTreeData);
  
    }

    public EncounterTreeData GetSaveData()
    {
        return new EncounterTreeData(this);
    }


    public void LoadData(EncounterTreeData encounterTreeData)
    {
       
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

    public void SetAllNodesMoveable(bool movable)
    {
        foreach (var c in columns)
        {
            foreach (var node in c.children)
            {
                node.SetMoveable(movable);
            }
        }
    }
}