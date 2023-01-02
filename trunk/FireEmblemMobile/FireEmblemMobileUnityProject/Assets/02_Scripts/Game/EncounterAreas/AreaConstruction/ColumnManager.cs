using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using __2___Scripts.Game.Areas;
using Game.Systems;
using LostGrace;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public struct FixedColumnNode
{
    public int columnIndex;
    public EncounterNodeData nodeData;
}
public class ColumnManager : MonoBehaviour
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
    public bool fixedEncounters = true;
    public List< FixedColumnNode> fixedColumns;




    //public float yRandom = 0.5f;
    
    // Start is called before the first frame update

    
  
    void Start()
    {
        //OnDisable();
        EncounterTree.Instance.spawnData = spawnData;
        EncounterTree.Instance.columns.Clear();
        if (LoadedSaveData())
        {
            Debug.Log("Load EncounterTreeData");
           
            EncounterTree.Instance.LoadData(SaveData.currentSaveData.encounterTreeData);
            
        }
        else
        {
            Debug.Log("Create New EncounterTree!");
            EncounterTree.Instance.spawnData.InitNodeAppearanceChances();
            EncounterTree.Instance.Create(fixedEncounters, fixedColumns);

        }
       
        EncounterTree.Instance.startNode.SetGameObject(CreateStartNodeGameObject(EncounterTree.Instance.startNode));
        CreateMiddleNodesGameObject(EncounterTree.Instance.columns);
        EncounterTree.Instance.endNode.SetGameObject(CreateEndNodeGameObject(EncounterTree.Instance.endNode));
        ConnectEncounters(EncounterTree.Instance.columns,EncounterTree.Instance.startNode, EncounterTree.Instance.endNode);
        PositionEncounters(EncounterTree.Instance.columns, EncounterTree.Instance.endNode);
        CreateConnections(EncounterTree.Instance.columns);
      

    }

    private bool LoadedSaveData()
    {
        return SaveData.currentSaveData != null && SaveData.currentSaveData.encounterTreeData != null;
    }

    private void CreateMiddleNodesGameObject(List<Column> columns)
    {
        
        for (int i=1; i < columns.Count-1; i++)
        {
            foreach (var node in columns[i].children)
            {
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
    
    private GameObject CreateEndNodeGameObject(EncounterNode node)
    {
        var go = GameObject.Instantiate(spawnData.endNodeData.prefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.name = "End Node";
        go.GetComponentInChildren<EncounterNodeClickController>().encounterNode = node;
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

    void ConnectEncounters(List<Column> columns, EncounterNode startNode, EncounterNode endNode)
    {
        
        for (int i = 0; i < columns[0].children.Count; i++)
        {
            columns[0].children[i].parents.Add(startNode);
            startNode.children.Add(columns[0].children[i]);
        }
        
        for (int i = 1; i < spawnData.columnCount; i++)
        {
            int prevChildCount = columns[i - 1].children.Count;
            int childCount = columns[i].children.Count;
            var prevColumn = columns[i - 1];
            var currColumn = columns[i];
            for (int o = 0; o < (childCount>prevChildCount?childCount/2f:prevChildCount/2f); o++)
            {
                if (o < prevChildCount&& o < childCount)
                {
                     int randomMin = Math.Max(0,  o - 1);
                     int randomMax = Math.Min(o+1, prevColumn.children.Count-1);
                     int randomParentIndex = Random.Range(randomMin, randomMax+1);
                    currColumn.children[o].AddParent(prevColumn.children[o]);
                    currColumn.children[o].AddParent(prevColumn.children[randomParentIndex]);
                     randomMin = Math.Max(0, childCount - 1 - o - 1);
                     randomMax = Math.Min(childCount - 1 - o+1, prevColumn.children.Count-1);
                     randomParentIndex = Random.Range(randomMin, randomMax+1);
                    currColumn.children[childCount - 1 - o].AddParent(prevColumn.children[prevChildCount - 1 - o]);
                    currColumn.children[childCount - 1 - o].AddParent(prevColumn.children[randomParentIndex]);
                }
            }

            for (int j = 0; j < columns[i].children.Count; j++)
            {
                if (columns[i].children[j].parents.Count == 0)
                {
                    int randomMin = Math.Max(0, j - 1);
                    int randomMax = Math.Min(j+1,columns[i-1].children.Count-1);
                    int randomParentIndex = Random.Range(randomMin, randomMax+1);
                    columns[i].children[j].AddParent(columns[i-1].children[randomParentIndex]);
                }
            }
        }
    }

    void PositionEncounters(List<Column> columns, EncounterNode endNode)
    {
        for (int i = 1; i < spawnData.columnCount; i++)
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
                }
            }
        }
        for (int j = 0; j < columns[spawnData.columnCount - 1].children.Count; j++)
        {
        
            columns[spawnData.columnCount - 1].children[j].children.Add(endNode);     
            endNode.gameObject.transform.localPosition = new Vector3(
                spawnData.columnCount * (columnWidth*1.0f),  0);
        }
    }
    

    

    
    private void CreateNodeGameObject(GameObject prefab, EncounterNode node, int index)
    {
        var go = GameObject.Instantiate(prefab, this.transform);
        go.name = "EncounterNode Column:" + index;
        node.SetGameObject(go);
       // Debug.Log("Create Node: " + node + " Index: " + index);
        go.GetComponentInChildren<EncounterNodeClickController>().encounterNode = node;
    }
    

    

   
}
