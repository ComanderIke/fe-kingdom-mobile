using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class EncounterNode
{
    public string UniqueId { get; set; }
    public List<EncounterNode> parents;
    public List<EncounterNode> children;

    public GameObject gameObject;

    public bool moveable;
    //public Column column;

    public EncounterNode(EncounterNode parent)
    {
        children = new List<EncounterNode>();
        parents = new List<EncounterNode>();
        parents.Add(parent);
    }

    
}



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
    public GameObject NodePrefab;
    private EncounterNode startNode;
    private EncounterNode endNode;
    public GameObject LineRendererPrefab;
    private List<Column> columns = new List<Column>();
    private List<GameObject> connections = new List<GameObject>();
    public float xRandomMin = -0.5f;
    public float xRandomMax = 0.5f;
    public float yRandomMin = -0.5f;
    public float yRandomMax = 0.5f;
    public int battleEncountersUntilDifferentNode = 1;

    public GameObject startEncounterPrefab;
    public GameObject endEncounterPrefab;
    public GameObject ChurchNodePrefab;
    public float ChurchNodeChance = 0.1f;
    public GameObject InnNodePrefab;
    public float InnNodeChance = 0.35f;
    public GameObject MerchantNodePrefab;
    public float MerchantNodeChance = 0.15f;
    public GameObject SmithyNodePrefab;
    public float SmithyNodeChance = 0.10f;
    public GameObject BattleNodePrefab;
    public float BattleNodeChance = 0.35f;
    public GameObject BattleNodePrefab2;
    public float BattleNode2Chance = 0.15f;
    public GameObject TreasureEncounterPrefab;
    public float TreasureNodeChance = 0.05f;
    public GameObject EventEncounterPrefab;
    public float EventNodeChance = 0.05f;
    public float ChanceDistributionAfterOccurence = 0.05f;
    private Dictionary<string, float> StartEncounterChances = new Dictionary<string, float>();
    private Dictionary<string, float> EncounterChances = new Dictionary<string, float>();

    private float sumAllStartChances;
    //public float yRandom = 0.5f;
    
    // Start is called before the first frame update

    void OnEnable()
    {
        OnDisable();
        EncounterChances.Clear();
        EncounterChances.Add(nameof(SmithyNodeChance), SmithyNodeChance);
        EncounterChances.Add(nameof(InnNodeChance), InnNodeChance);
        EncounterChances.Add(nameof(ChurchNodeChance), ChurchNodeChance);
        EncounterChances.Add(nameof(MerchantNodeChance), MerchantNodeChance);
        EncounterChances.Add(nameof(BattleNodeChance), BattleNodeChance);
        EncounterChances.Add(nameof(BattleNode2Chance), BattleNode2Chance);
        EncounterChances.Add(nameof(TreasureNodeChance), TreasureNodeChance);
        EncounterChances.Add(nameof(EventNodeChance), EventNodeChance);
        StartEncounterChances = new Dictionary<string, float>(EncounterChances);
        columns.Clear();
        sumAllStartChances = 0;
        foreach (var keyvaluepair in EncounterChances)
        {
            Debug.Log(keyvaluepair.Key+" "+keyvaluepair.Value);
            sumAllStartChances += keyvaluepair.Value;
        }
        
        Column startColumn = new Column();
        startNode = new EncounterNode(null);
        startColumn.children.Add(startNode);
        startColumn.index = 0;
       
        
        var go = GameObject.Instantiate(startEncounterPrefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.GetComponentInChildren<EncounterClickController>().encounterNode = startNode;
        go.name = "Start Node";
        startNode.gameObject = go;
        columns.Add(startColumn);
        
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
        Column endColumn = new Column();
        endNode = new EncounterNode(null);
        endColumn.children.Add(endNode);
        endColumn.index = columnCount;
        go = GameObject.Instantiate(endEncounterPrefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.name = "End Node";
        go.GetComponentInChildren<EncounterClickController>().encounterNode = endNode;
        endNode.gameObject = go;
        
        columns.Add(endColumn);
        PositionEncounters();
        CreateConnections();
        foreach (var keyvaluepair in EncounterChances)
        {
            Debug.Log(keyvaluepair.Key+" "+keyvaluepair.Value);
        }

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
                    //Debug.Log("X Pos: "+columns[i].index * columnWidth);
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
       // Debug.Log("Current: "+current.index+" Previous: "+columns[current.index - 1].index+" Children: "+columns[current.index - 1].children.Count);
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

    

    void SpawnSingleEncounter(EncounterNode parent, Column current)
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
        if (!bindChild)
        {
            EncounterNode node = new EncounterNode(parent);
            float rng = Random.Range(0f, ChurchNodeChance+BattleNodeChance+BattleNode2Chance+InnNodeChance+MerchantNodeChance+SmithyNodeChance);
            var prefab = NodePrefab;
            if (current.index <= battleEncountersUntilDifferentNode)
            {
                rng += ChurchNodeChance + InnNodeChance + MerchantNodeChance + SmithyNodeChance;
            }

            float threshold = 0;
            string chosenKey = "";
            foreach (var key in EncounterChances.Keys)
            {
                threshold +=EncounterChances[key];
                if (rng <= threshold)
                {
                    UpdateEncounterChances(key);
                    chosenKey = key;
                    break;
                }
                    
            }

            switch (chosenKey)
            {
                case nameof(InnNodeChance): 
                    prefab = InnNodePrefab;break;
                case nameof(ChurchNodeChance): 
                    prefab = ChurchNodePrefab;break;
                case nameof(SmithyNodeChance): 
                    prefab = SmithyNodePrefab;break;
                case nameof(MerchantNodeChance): 
                    prefab = MerchantNodePrefab;break;
                case nameof(BattleNodeChance): 
                    prefab = BattleNodePrefab;break;
                case nameof(BattleNode2Chance): 
                    prefab = BattleNodePrefab2;break;
                case nameof(TreasureNodeChance): 
                    prefab = TreasureEncounterPrefab;break;
                case nameof(EventNodeChance): 
                    prefab = EventEncounterPrefab;break;
                
            }
            var go = GameObject.Instantiate(prefab, this.transform);
            go.name = "EncounterNode Column:" + current.index;
            node.gameObject = go;
            go.GetComponentInChildren<EncounterClickController>().encounterNode = node;
            current.children.Add((node));
            parent.children.Add(node);
            //Debug.Log("Spawn New Node!");
        }
    }

    
    private void UpdateEncounterChances(string paramkey)
    {
        List<string> keys = new List<string>(EncounterChances.Keys);
        foreach (var key in keys)
        {
            if (key.CompareTo(paramkey)==0)
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

    void SpawnSpecificEncounter(EncounterNode parent, Column current, GameObject nodePrefab)
    {

            EncounterNode node = new EncounterNode(parent);
            var go = GameObject.Instantiate(nodePrefab, this.transform);
            go.name = "EncounterNode Column:" + current.index;
            node.gameObject = go;
            current.children.Add((node));
            parent.children.Add(node);
            //Debug.Log("Spawn New Node!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EncounterNode GetStartNode()
    {
        return startNode;
    }
}
