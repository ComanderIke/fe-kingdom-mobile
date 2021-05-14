using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEditor.MemoryProfiler;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LocationRenderer))]
public class WorldMapPosition : MonoBehaviour
{
    public WorldMapPosition[] Connections;


    private List<Road> roads;

    private WM_Actor actor;
    public WM_Actor Actor
    {
        get
        {
            return actor;
        }
        set
        {
            if(actor!=null)
                actor.TurnStateManager.onSelected -= OnSelectedActor;
            actor = value;
            if (actor != null)
                actor.TurnStateManager.onSelected += OnSelectedActor;
        }
    }

  
    public  IWorldMapLocationInputReceiver inputReceiver { get; set; }

    public LocationRenderer renderer{ get; set; }
    private void OnEnable()
    {
        renderer = GetComponent<LocationRenderer>();
        spawndirty = false;
    }

    void OnSelectedActor(bool selected)
    {
        
        if(selected)
            renderer.DrawInteractableConnections();
        else
            renderer.HideInteractableConnections();

    }
    
    public void OnMouseDown()
    {
        inputReceiver.LocationClicked(this);
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

    
    

  

    public bool IsFree()
    {
        return actor == null;
    }

    public bool IsReachable(WM_Actor selectedActor)
    {
        Debug.Log(selectedActor.location+" "+this);
       return selectedActor.location.Connections.Contains(this);
    }

    public bool IsAttackable(WM_Actor selectedActor)
    {
        return selectedActor.location.Connections.Contains(this);
    }

    public void Select(bool selected)
    {
        renderer.ShowSelected(selected);
    }

    public void Reset()
    {
        renderer.Reset();
        foreach (var connection in Connections)
        {
            connection.renderer.Reset();
        }
    }
}