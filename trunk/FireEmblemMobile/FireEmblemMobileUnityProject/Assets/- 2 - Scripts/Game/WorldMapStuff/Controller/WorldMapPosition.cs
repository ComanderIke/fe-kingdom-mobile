using System;
using System.Collections.Generic;
using System.Linq;
using Game.WorldMapStuff.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.WorldMapStuff.Controller
{
    [ExecuteInEditMode]
    public class WorldMapPosition : MonoBehaviour
    {
        public LocationController[] locationControllers;
        public WorldMapPosition[] Connections;

        public int space = 2;

        private List<Road> roads;
        
        private bool dirty = false;
        private bool spawndirty = false;
        private void OnEnable()
        {
      
            spawndirty = false;
            locationControllers[0].SetActive(true);
        }

        public void UpdatePositionLocations()
        {
            int cnt = 0;
            foreach (var loc in locationControllers)
            {
                if (loc.IsActive())
                    cnt++;
            }

            if (cnt == 0)
            {
                cnt = 1;
                locationControllers[0].SetActive(true);
            }

            int length = cnt;
            float halfDistance = ((length-1) / 2.0f)*transform.localScale.x;
          //  Debug.Log("length: "+length+" halfdistance: "+halfDistance);
            for (int i = 0; i < length; i++)
            {
                locationControllers[i].SetPosition(transform.position.x-halfDistance + i* locationControllers[0].renderer.spriteRenderer.size.x*transform.localScale.x);
                
            }
            
        }
        public bool IsReachable(WM_Actor selectedActor)
        {
            return selectedActor.location.worldMapPosition.Connections.Contains(this);
        }

        public bool IsAttackable(WM_Actor selectedActor)
        {
            return IsReachable(selectedActor);
        }
        public bool HasSpace()
        {
            foreach (LocationController location in locationControllers)
            {
                if (location.IsFree())
                    return true;
            }

            return false;
        }
        
        #region Roads
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
        
       #endregion

       public void SetActor(Party startingParty)
       {
           foreach (var loc in locationControllers)
           {
               if (loc.IsFree())
               {
                   loc.Actor = startingParty;
                   break;
               }
           }
       }
       public void Reset()
    {
        foreach (var loc in locationControllers)
        {
            loc.Reset();
        }
        
        foreach (var connection in Connections)
        {
            foreach (var loc in connection.locationControllers)
            {
                loc.Reset();
            }
        }
    }
       public void DrawInteractableConnections()
       {
           Debug.Log("Draw Interactables:");
        

           foreach (WorldMapPosition connection in Connections)
           {
               foreach (var loc in connection.locationControllers)
               {
                   if (loc.IsActive())
                   {
                       if (loc.Actor != null && !loc.Actor.Faction.IsActive())
                       {
                           loc.renderer.ShowEnemy();
                       }
                       else
                       {
                           loc.renderer.ShowWalkable();
                       }
                   }
               }

           }
       }
       public void HideInteractableConnections()
       {
           foreach (var loc in locationControllers)
           {
               loc.Hide();
           }
           foreach (WorldMapPosition connection in Connections)
           {
               foreach (var loc in connection.locationControllers)
               {
                   loc.Hide();
               }
           }
       }

       public void Hide()
       {
           foreach (var loc in locationControllers)
               loc.Hide();
       }


       public List<WM_Actor> GetActors()
       {
           List<WM_Actor> actors = new List<WM_Actor>();
           foreach (var loc in locationControllers)
           {
               if(loc.Actor!=null)
                actors.Add(loc.Actor);
           }

           return actors;
       }
    }
}