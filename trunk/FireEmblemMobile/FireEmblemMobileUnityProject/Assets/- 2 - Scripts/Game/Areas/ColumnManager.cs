using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using __2___Scripts.Game.Areas;
using Game.Systems;
using Pathfinding;
using UnityEngine;
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
           
            EncounterTree.Instance.CreateFromData(SaveData.currentSaveData.encounterTreeData);
            
        }
        else
        {
            Debug.Log("Create New EncounterTree!");
            EncounterTree.Instance.spawnData.InitNodeAppearanceChances();
            EncounterTree.Instance.Create(fixedEncounters, fixedColumns);

        }
       
        EncounterTree.Instance.startNode.gameObject=CreateStartNodeGameObject(EncounterTree.Instance.startNode);
        CreateMiddleNodesGameObject(EncounterTree.Instance.columns);
        EncounterTree.Instance.endNode.gameObject= CreateEndNodeGameObject(EncounterTree.Instance.endNode);
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
                CreateNodeGameObject(spawnData.nodeDatas[node.prefabIdx].prefab, node, i);
            }
        }
        
    }

   

    private GameObject CreateStartNodeGameObject(EncounterNode node)
    {
        var go = GameObject.Instantiate(spawnData.startNodeData.prefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.GetComponentInChildren<EncounterClickController>().encounterNode = node;
        go.name = "Start Node";
        return go;
    }
    
    private GameObject CreateEndNodeGameObject(EncounterNode node)
    {
        var go = GameObject.Instantiate(spawnData.endNodeData.prefab, this.transform);
        go.transform.localPosition = Vector3.zero;
        go.name = "End Node";
        go.GetComponentInChildren<EncounterClickController>().encounterNode = node;
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
        node.gameObject = go;
        Debug.Log("Create Node: " + node + " Index: " + index);
        go.GetComponentInChildren<EncounterClickController>().encounterNode = node;
    }
    

    

   
}
