using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class Column
{
    public List<EncounterNode> children;
    public int index;

    public Column()
    {
        children = new List<EncounterNode>();
    }
}

public class ColumnManager : MonoBehaviour
{
    public int columnCount=10;
    public float columnWidth= 5f;
    public float columnHeight= 2f;
    public float encounter2ChildPercentage=0.3f;
    public float encounter3ChildPercentage=0.2f;
    public float chanceToShareChild = 0.3f;
    public int columnMaxEncounter = 5;
    private EncounterNode startNode;
    private EncounterNode endNode;
    public GameObject LineRendererPrefab;
    private List<Column> columns = new List<Column>();
    private List<GameObject> connections = new List<GameObject>();
    public float xRandomMin = -0.5f;
    public float xRandomMax = 0.5f;
    public float yRandomMin = -0.5f;
    public float yRandomMax = 0.5f;

    public EncounterNodeData startNodeData;
    public List<EncounterNodeData> nodeDatas;
    public EncounterNodeData endNodeData;

    public float ChanceDistributionAfterOccurence = 0.05f;
    private Dictionary<EncounterNodeData, float> EncounterChances = new Dictionary<EncounterNodeData, float>();
    private Dictionary<EncounterNodeData, float> StartEncounterChances = new Dictionary<EncounterNodeData, float>();
    private float sumAllStartChances;
    //public float yRandom = 0.5f;
    
    // Start is called before the first frame update

  
    void OnEnable()
    {
        OnDisable();
        EncounterChances.Clear();
        StartEncounterChances.Clear();
        foreach (var encounterData in nodeDatas)
        {
            Debug.Log("add: " + encounterData);
            EncounterChances.Add(encounterData, encounterData.appearanceChance);
            StartEncounterChances.Add(encounterData, encounterData.appearanceChance);
        }
        columns.Clear();
        sumAllStartChances = 0;
        foreach (var keyvaluepair in EncounterChances)
        {
            sumAllStartChances += keyvaluepair.Value;
        }
        CreateStartColumn();
        CreateMiddleColumns();
        CreateEndColumn();
        PositionEncounters();
        CreateConnections();

    }

    private void CreateMiddleColumns()
    {
        for (int i = 1; i < columnCount; i++)
        {
            Column column = new Column();
            column.index = i;
            //Debug.Log("Create Column!");
            //Debug.Log(columns.Last().children.Count);
            foreach (EncounterNode node in columns.Last().children)
            {
                //Debug.Log("Spawn Encounter!"+ node+" "+column);
                SpawnEncounters(node, column);
            }
            
            columns.Add(column);
        }
    }
    void CreateStartColumn()
    {
        Column startColumn = new Column();

        var go = GameObject.Instantiate(startNodeData.prefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        startNode = startNodeData.CreateNode(null);
        
        go.GetComponentInChildren<EncounterClickController>().encounterNode = startNode;
        go.name = "Start Node";
        startColumn.children.Add(startNode);
        startColumn.index = 0;
        startNode.gameObject = go;
        columns.Add(startColumn);
    }

    void CreateEndColumn()
    {
        Column endColumn = new Column();
        if (endNodeData is BattleEncounterNodeData data)
        {
            endNode = new BattleEncounterNode(data.levelIndex, data.EnemyArmyData,null);
        }

        endColumn.children.Add(endNode);
        endColumn.index = columnCount;
        var go = GameObject.Instantiate(endNodeData.prefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.name = "End Node";
        go.GetComponentInChildren<EncounterClickController>().encounterNode = endNode;
        endNode.gameObject = go;
        
        columns.Add(endColumn);
    }

    private void CreateConnections()
    {
        
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < columns[i].children.Count; j++)
            {

                for (int k = 0; k < columns[i].children[j].children.Count; k++)
                {
                    var go = Instantiate(LineRendererPrefab, columns[i].children[j].gameObject.transform);

                    UpdateLineRenderer(go.GetComponent<LineRenderer>(),
                        columns[i].children[j].gameObject.transform.position, columns[i].children[j].children[k].gameObject.transform.position);
                }
            }
        }
    }
    private void UpdateLineRenderer(LineRenderer lineRenderer,Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.positionCount = 2;
        Vector3[] pos = new Vector3[2];
        pos[0] = startPos;
        pos[1] = endPos;
        lineRenderer.SetPositions(pos);
    }

    private void OnDisable()
    {
        DeleteExistingGameObjects();
        DeleteConnections();
    }
    void DeleteConnections()
    {
        if (connections == null)
            return;
        
       
        for (int j = connections.Count - 1; j >= 0; j--)
        {
            DestroyImmediate(connections[j]);
        }
    }
    void DeleteExistingGameObjects()
    {
        if (columns == null)
            return;
        
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = columns[i].children.Count - 1; j >= 0; j--)
            {
                DestroyImmediate(columns[i].children[j].gameObject);
            }

        }
    }
    void PositionEncounters()
    {
        for (int i = 1; i < columnCount; i++)
        {
            for (int j = 0; j < columns[i].children.Count; j++)
            {
               
               // Debug.Log("Position Encounter: ");
                if (columns[i].children[j].gameObject != null)
                {
                    if (columns[i].children.Count == 1)
                    {
                        columns[i].children[j].gameObject.transform.localPosition = new Vector3(
                            columns[i].index * (columnWidth*1.0f) + Random.Range(xRandomMin, xRandomMax),  Random.Range(yRandomMin, yRandomMax));
                    }
                    else
                    {
                        columns[i].children[j].gameObject.transform.localPosition = new Vector3(
                            columns[i].index * columnWidth+ Random.Range(xRandomMin, xRandomMax),
                            (j * (columnHeight*1.0f / (columns[i].children.Count-1)) -
                            (columnHeight*1.0f / 2)+ Random.Range(yRandomMin, yRandomMax)));
                    }
                }
            }
        }
        for (int j = 0; j < columns[columnCount - 1].children.Count; j++)
        {
        
            columns[columnCount - 1].children[j].children.Add(endNode);     
            endNode.gameObject.transform.localPosition = new Vector3(
                columnCount * (columnWidth*1.0f),  0);
        }
    }
    void SpawnEncounters(EncounterNode parent, Column current)
    {
        float rng = Random.value;
        if (columns[current.index - 1].children.Count == 1)
        {
            rng = Random.Range(0, encounter2ChildPercentage);
        }
        if (columns[current.index - 1].children.Count >= 4)
        {
            rng += encounter3ChildPercentage;
        }
        if (rng <= encounter3ChildPercentage)
        {
            //Debug.Log("Spawn Triple Node");
            SpawnSingleEncounter(parent, current);
            SpawnSingleEncounter(parent, current);
            SpawnSingleEncounter(parent, current);
        }
        else if (rng <= encounter2ChildPercentage)
        {
            
            //Debug.Log("Spawn Double Node");
           SpawnSingleEncounter(parent, current);
           SpawnSingleEncounter(parent, current);

        }
        else
        {
            //Debug.Log("Spawn Single Node");
            SpawnSingleEncounter(parent, current);
        }
    }

    bool ShareChild(EncounterNode parent, Column current)
    {
        bool bindChild = false;
        float chance = chanceToShareChild;
        if (current.children.Count > 0)
        {
            float rng2 = Random.value;
            if (columns[current.index - 1].children.Count >= 3)
            {
                chance *= 2;
            }
            if (rng2 <= chance|| current.children.Count + 1 > columnMaxEncounter)
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
    void SpawnSingleEncounter(EncounterNode parent, Column current)
    {
        if (!ShareChild(parent, current))
        {
            
            float threshold = 0;
            EncounterNodeData chosenKey =null;
            float sumAllChances = 0;
            foreach (var key in EncounterChances.Keys)
            {
                sumAllChances += EncounterChances[key];
            }

            float rng = Random.Range(0, sumAllChances);
            //Debug.Log(sumAllStartChances);
            foreach (var key in EncounterChances.Keys)
            {
                threshold +=EncounterChances[key];
                Debug.Log(key+ " "+threshold+" "+rng);
                if (rng <= threshold)
                {
                    UpdateEncounterChances(key);
                    chosenKey = key;
                    break;
                }
                    
            }
            
            EncounterNode node = chosenKey.CreateNode(parent);

            var go = GameObject.Instantiate(chosenKey.prefab, this.transform);
            go.name = "EncounterNode Column:" + current.index;
            node.gameObject = go;
            go.GetComponentInChildren<EncounterClickController>().encounterNode = node;
            current.children.Add((node));
            parent.children.Add(node);
            //Debug.Log("Spawn New Node!");
        }
    }

    
    private void UpdateEncounterChances(EncounterNodeData paramkey)
    {
        List<EncounterNodeData> keys = new List<EncounterNodeData>(EncounterChances.Keys);
        foreach (var key in keys)
        {
            if (key==paramkey)
            {
                EncounterChances[key] = StartEncounterChances[key]-ChanceDistributionAfterOccurence;
                 if (EncounterChances[key] < 0)
                     EncounterChances[key] = 0;
            }
            else
            {
                EncounterChances[key] += ChanceDistributionAfterOccurence*(StartEncounterChances[key]/sumAllStartChances);
            }
        }
    }

    public EncounterNode GetStartNode()
    {
        return startNode;
    }
}
