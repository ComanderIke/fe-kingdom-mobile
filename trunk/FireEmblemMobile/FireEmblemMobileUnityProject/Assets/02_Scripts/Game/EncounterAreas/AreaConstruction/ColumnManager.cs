using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using __2___Scripts.Game.Areas;
using Game.GameActors.Players;
using Game.GameResources;
using Game.Systems;
using LostGrace;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]

public class ColumnManager : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    public EncounterSpawnData spawnData;
    
    public float columnWidth= 5f;
    public float columnHeight= 2f;

    public GameObject LineRendererPrefab;
    
    private List<GameObject> connections = new List<GameObject>();
    public float xRandomMin = -0.7f;
    public float xRandomMax = 0.7f;
    public float yRandomMin = -0.4f;
    public float yRandomMax = 0.4f;
    [SerializeField] float columspacing = 1.5f;
    




    //public float yRandom = 0.5f;
    
    // Start is called before the first frame update

    private void OnDestroy()
    {
        SaveGameManager.UnregisterDataPersistanceObject(this);
    }

    private void Awake()
    {
        SaveGameManager.RegisterDataPersistanceObject(this);
    }

    void Start()
    {
        //OnDisable();
        if (AreaGameManager.Instance.CampaingFinished)
            return;
        if(!SaveGameManager.HasEncounterSaveData()||Player.Instance.Party.AreaIndex!=EncounterTree.Instance.AreaIndex)
        {
           
            EncounterTree.Instance.AreaIndex = Player.Instance.Party.AreaIndex;
            EncounterTree.Instance.spawnData = spawnData;
            EncounterTree.Instance.columns.Clear();
            //Debug.Log(SaveGameManager.currentSaveData.encounterTreeData.columns.Count);
            MyDebug.LogLogic("Create New EncounterTree!");
            //No more Areas implemented for now
           
            EncounterTree.Instance.spawnData.InitNodeAppearanceChances();
            EncounterTree.Instance.Create();
            
        
        }
     
        //
        EncounterTree.Instance.startNode.SetGameObject(CreateStartNodeGameObject(EncounterTree.Instance.startNode));
        CreateMiddleNodesGameObject(EncounterTree.Instance.columns);
        PositionEncounters(EncounterTree.Instance.columns);
        ConnectEncounters(EncounterTree.Instance.columns);
        CreateConnections(EncounterTree.Instance.columns);
      

    }

    

    private void CreateMiddleNodesGameObject(List<Column> columns)
    {
        for (int i=1; i < columns.Count; i++)
        {
            foreach (var node in columns[i].children)
            {
                MyDebug.LogTest(node.prefabIdx+" "+spawnData.allNodeDatas.Count);
                CreateNodeGameObject(spawnData.allNodeDatas[node.prefabIdx].prefab, node, i);
            }
        }
    }

   

    private GameObject CreateStartNodeGameObject(EncounterNode node)
    {
        var go = GameObject.Instantiate(spawnData.startNodeData.prefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.GetComponentInChildren<EncounterNodeClickController>().encounterNode = node;
        go.name = "Start Node";
        return go;
    }
    
    
    private void CreateConnections(List<Column> columns)
    {
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < columns[i].children.Count; j++)
            {
                for (int k = 0; k < columns[i].children[j].children.Count; k++)
                {
                    var go = Instantiate(LineRendererPrefab, columns[i].children[j].gameObject.transform);
                    Road road = go.GetComponent<Road>();
                    road.SetStartNode(columns[i].children[j]);
                    road.end = columns[i].children[j].children[k];
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
  
    void ConnectEncounters(List<Column> columns)
    {
        for (int i = 0; i < spawnData.GetColumnCount(Player.Instance.Party.AreaIndex)-1; i++)
        {

            foreach (int j in Enumerable.Range(0, columns[i].children.Count)
                         .OrderBy(x => Random.value)) //Iterate through nodes randomly
            {
                var currentNode = columns[i].children[j];
                var yPos = currentNode.gameObject.transform.position.y;
              

                var min = columns[i + 1].children.Aggregate((curMin, x) =>
                    (Math.Abs(Math.Abs(x.gameObject.transform.position.y - yPos) -
                              Math.Abs(curMin.gameObject.transform.position.y - yPos)) < 0.1f
                        ?
                        (Random.value >= .5f ? x : curMin)//if distance is equal choose randomly
                        : Math.Abs(x.gameObject.transform.position.y - yPos) <
                          Math.Abs(curMin.gameObject.transform.position.y - yPos)
                            ? x
                            : curMin));
                    

                currentNode.AddChild(min);
            }

            foreach (var child in columns[i + 1].children)
            {
                if (child.parents.Count == 0)
                {
                    var yPos = child.gameObject.transform.position.y;
                    
                    var min = columns[i].children.Aggregate((curMin, x) =>
                        (Math.Abs(Math.Abs(x.gameObject.transform.position.y - yPos) -
                                  Math.Abs(curMin.gameObject.transform.position.y - yPos)) < 0.1f
                            ?
                            (Random.value >= .5f ? x : curMin)//if distance is equal choose randomly
                            : Math.Abs(x.gameObject.transform.position.y - yPos) <
                              Math.Abs(curMin.gameObject.transform.position.y - yPos)
                                ? x
                                : curMin));
                    child.AddParent(min);
                }
            }

            foreach (int j in Enumerable.Range(0, columns[i].children.Count)
                         .OrderBy(x => Random.value))
            {
                var currentNode = columns[i].children[j];
                var yPos = currentNode.gameObject.transform.position.y;
                foreach (var child in columns[i + 1].children)
                {
                    if (Random.value > 0.6f)
                    {
                        var childYPos = child.gameObject.transform.position.y;
                        if (child.parents.Contains(currentNode))
                            continue;
                        bool hasChildThatCrosses= false;
                        foreach (var parent in columns[i].children)
                        {
                            if(parent==currentNode)
                                continue;
                            if (parent.gameObject.transform.position.y > yPos)//above sibling
                            {
                                //check if above parents have a child beneath this child
                               
                                foreach (var parentChild in parent.children)
                                {
                                    if (parentChild.gameObject.transform.position.y < childYPos)
                                    {
                                        hasChildThatCrosses = true;
                                        break;
                                    }
                                }

                                if (hasChildThatCrosses)
                                    break;

                            }
                            else // underneith sibling
                            {
                                foreach (var parentChild in parent.children)
                                {
                                    if (parentChild.gameObject.transform.position.y > childYPos)
                                    {
                                        hasChildThatCrosses = true;
                                        break;
                                    }
                                }

                                if (hasChildThatCrosses)
                                    break;
                            }
                        }
                        if(!hasChildThatCrosses)
                            currentNode.AddChild(child);
                    }
                    
                }
                
            }
        }


        //     int prevChildCount = columns[i - 1].children.Count;
        //     int childCount = columns[i].children.Count;
        //     var prevColumn = columns[i - 1];
        //     var currColumn = columns[i];
        //     
        //     for (int o = 0; o < (childCount>prevChildCount?childCount/2f:prevChildCount/2f); o++)
        //     {
        //         if (o < prevChildCount&& o < childCount)
        //         {
        //              int randomMin = Math.Max(0,  o - 1);
        //              int randomMax = Math.Min(o+1, prevColumn.children.Count-1);
        //              int randomParentIndex = Random.Range(randomMin, randomMax+1);
        //              currColumn.children[o].AddParent(prevColumn.children[o]);
        //             currColumn.children[o].AddParent(prevColumn.children[randomParentIndex]);
        //              randomMin = Math.Min( prevColumn.children.Count-1, childCount - 1 - o - 1);
        //              randomMin = Math.Max(randomMin, 0);
        //              randomMax = Math.Max(childCount - 1 - o+1,0);
        //              randomMax = Math.Min(prevChildCount - 1, randomMax);
        //              randomParentIndex = Random.Range(randomMin, randomMax+1);
        //              currColumn.children[childCount - 1 - o].AddParent(prevColumn.children[prevChildCount - 1 - o]);
        //             currColumn.children[childCount - 1 - o].AddParent(prevColumn.children[randomParentIndex]);
        //         }
        //     }
        //
        //     for (int j = 0; j < columns[i].children.Count; j++)
        //     {
        //         if (columns[i].children[j].parents.Count == 0)
        //         {
        //             int randomMin = Math.Max(0, j - 1);
        //             int randomMax = Math.Min(j+1,columns[i-1].children.Count-1);
        //             int randomParentIndex = Random.Range(randomMin, randomMax+1);
        //             columns[i].children[j].AddParent(columns[i-1].children[randomParentIndex]);
        //         }
        //     }
        // }
    }

    void PositionEncounters(List<Column> columns)
    {
        for (int i = 1; i < spawnData.GetColumnCount(Player.Instance.Party.AreaIndex); i++)
        {
            for (int j = 0; j < columns[i].children.Count; j++)
            {
               
               // Debug.Log("Position Encounter: ");
                if (columns[i].children[j].gameObject != null)
                {
                    if (columns[i].children.Count == 1)
                    {
                        columns[i].children[j].gameObject.transform.localPosition = new Vector3(
                            columns[i].index * (columnWidth*1.0f) ,  0);
                    }
                    else
                    {
                        
                        float direction = j % 2 == 0 ? 1 : -1;
                        float maxDistance = -(columns[i].children.Count-1)/2f*columspacing;
                       
                        columns[i].children[j].gameObject.transform.localPosition = new Vector3(
                            columns[i].index * columnWidth,maxDistance+
                            (j * columspacing));
                    }
                    columns[i].children[j].gameObject.transform.Translate(new Vector3(Random.Range(xRandomMin, xRandomMax), Random.Range(yRandomMin, yRandomMax),0));
                }
            }
        }
        
    }
    

    

    
    private void CreateNodeGameObject(GameObject prefab, EncounterNode node, int index)
    {
        var go = GameObject.Instantiate(prefab, this.transform);
        go.name = node.label+" Column:" + index;
        if (node is EventEncounterNode eventNode)
            go.name = eventNode.randomEvent.name + " Event Column: " + index;
        node.SetGameObject(go);
       
        //Debug.Log("Create Node: " + node + " Index: " + index);
        go.GetComponentInChildren<EncounterNodeClickController>().encounterNode = node;
    }


    public void LoadData(SaveData data)
    {
        EncounterTree.Instance.spawnData = spawnData;
        EncounterTree.Instance.columns.Clear();
        if(SaveGameManager.HasEncounterSaveData())
            EncounterTree.Instance.LoadData(data.encounterTreeData);
        
    }

    public void SaveData(ref SaveData data)
    {
        data.encounterTreeData = EncounterTree.Instance.GetSaveData();
    }

   
}
