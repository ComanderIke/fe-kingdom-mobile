using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteInEditMode]
public class WorldMapPosition : MonoBehaviour
{
    public WorldMapPosition[] Connections;
    private List<Road> roads;

    private void OnEnable()
    {
        spawndirty = false;
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
    public void CreateRoads(GameObject roadPrefab)
    {
        if (spawndirty)
        {
            return;
        }

        roads ??= new List<Road>();
        spawndirty = true;
        foreach (var road in roads)
        {
            road.DeleteIfLineRendererIsNull();
        }
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
                roadGo.Spawn(roadPrefab, transform);
                
            }
            connection.CreateRoads(roadPrefab);
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
        foreach (var connection in Connections)
        {
            connection.DrawRoads();
        }
        
    }
    
}

public class Road: IEquatable<Road>
{
    private WorldMapPosition location1;
    private WorldMapPosition location2;
    private LineRenderer lineRenderer;//TODO

    public Road(WorldMapPosition location1, WorldMapPosition location2)
    {
        this.location1 = location1;
        this.location2 = location2;
        location1.AddRoad(this);
        location2.AddRoad(this);
    }

    public void Spawn(GameObject roadPrefab, Transform parent)
    {
        Debug.Log("Spawn Road: "+location1.gameObject.name + " - " + location2.gameObject.name);
        var go = GameObject.Instantiate(roadPrefab, parent, false);
        lineRenderer = go.GetComponent<LineRenderer>();
        go.name = location1.gameObject.name + " - " + location2.gameObject.name;
        
        Draw();
    }

    public void Draw()
    {
        if(lineRenderer!=null)
            UpdateLineRenderer(lineRenderer, location1.transform.position, location2.transform.position );
    }
    private void UpdateLineRenderer(LineRenderer lineRenderer,Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.positionCount = 2;
        Vector3[] pos = new Vector3[2];
        pos[0] = startPos;
        pos[1] = endPos;
        lineRenderer.SetPositions(pos);
    }
    
    public bool Equals(Road other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(location1, other.location1) && Equals(location2, other.location2);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Road) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((location1 != null ? location1.GetHashCode() : 0) * 397) ^ (location2 != null ? location2.GetHashCode() : 0);
        }
    }

    public bool Contains(WorldMapPosition connection)
    {
        return location1==connection|| location2==connection;
    }

    public void DeleteIfLineRendererIsNull()
    {
        if (!Exists())
        {
            if(lineRenderer!=null)
                Object.DestroyImmediate(lineRenderer.gameObject);
        }
    }

    public bool Exists()
    {
        return lineRenderer != null && lineRenderer.gameObject != null;
    }
}
