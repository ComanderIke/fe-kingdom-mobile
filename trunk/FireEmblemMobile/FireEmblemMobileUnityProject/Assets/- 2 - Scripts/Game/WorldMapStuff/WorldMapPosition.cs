using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

[ExecuteInEditMode]
public class WorldMapPosition : MonoBehaviour
{
    public WorldMapPosition[] Connections;

    public GameObject Walkable;
    public GameObject Attackable;
    private List<Road> roads;

    private void OnEnable()
    {
        spawndirty = false;
    }

    public void ShowAttackable()
    {
        Attackable.SetActive(true);
        Walkable.SetActive(false);
    }
    public void ShowWalkable()
    {
        Walkable.SetActive(true);
        Attackable.SetActive(false);
    }
    public void OnMouseDown()
    {
        Debug.Log("Position Clicked!");
    }
    public void AddRoad(Road road)
    {
        roads ??= new List<Road>();
        roads.Add(road);
    }
    public void RemoveRoad(Road road)
    {
        if(roads.Contains(road))
            roads.Remove(road);
    }
    public void CreateRoads(GameObject roadPrefab, Transform roadParent)
    {
        if (spawndirty)
        {
            return;
        }

        roads ??= new List<Road>();
        spawndirty = true;
        roads.RemoveAll(item => item.Exists() == false);
        foreach (var connection in Connections)
        {
            bool roadExists = false;
            foreach (var road in roads)
            {
               
                if (road.Contains(connection))
                {
                    roadExists = true;
                    break;
                }
            }

            if (!roadExists)
            {
                var roadGo = new Road(this, connection);
                roadGo.Spawn(roadPrefab, roadParent);
                
            }
            connection.CreateRoads(roadPrefab, roadParent);
        }
    }

    private bool dirty = false;
    private bool spawndirty = false;

    public void LateUpdate()
    {
        dirty = false;
        spawndirty = false;
    }

    public void DrawRoads()
    {
        if (dirty)
            return;
        foreach (Road road in roads)
        {
            road.Draw();
            
        }

        dirty = true;
        if(Connections!=null)
            foreach (var connection in Connections)
            {
                connection.DrawRoads();
            }
        
    }

    public void DrawInteractableConnections()
    {
        foreach (WorldMapPosition connection in Connections)
        {
            connection.ShowWalkable();
        }
    }
    public void HideInteractableConnections()
    {
        foreach (WorldMapPosition connection in Connections)
        {
            connection.Hide();
        }
    }

    private void Hide()
    {
        Walkable.SetActive(false);
        Attackable.SetActive(false);
    }
}