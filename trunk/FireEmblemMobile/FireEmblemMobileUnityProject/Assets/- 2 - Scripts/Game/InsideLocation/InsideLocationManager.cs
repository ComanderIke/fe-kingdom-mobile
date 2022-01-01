using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Menu;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Game.WorldMapStuff.Manager
{
    public class InsideLocationManager : MonoBehaviour
    {
        // Start is called before the first frame update

        public GameObject UnitPrefab;
        private Party party;
        public AstarPath pathfinderGraph;
        //public AIPathSystem pathSystem;
        public BuildingManager BuildingManager;
        private List<GameObject> units;
        public void Start()
        {
            this.party = Player.Instance.Party;
            // BuildingManager.SpawnBuildings(SceneTransferData.Instance.LocationData.Village.buildings);
            // SpawnUnits();
            // pathfinderGraph.Scan();//TODO slow?
            
        }

        private void OnDestroy()
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                var unit = units[i];
                units.Remove(unit);
                Destroy(unit);
                
            }
        }
        private void SpawnUnits()
        {
            float width = pathfinderGraph.data.gridGraph.width*pathfinderGraph.data.gridGraph.nodeSize;
            float height = pathfinderGraph.data.gridGraph.depth*pathfinderGraph.data.gridGraph.nodeSize;
          
            Vector2 center = pathfinderGraph.data.gridGraph.center;
            units = new List<GameObject>();
            foreach (var member in party.members)
            {
                var unitGo=Instantiate(UnitPrefab);
                unitGo.name = member.name;
                unitGo.GetComponentInChildren<SpriteRenderer>().sprite = member.visuals.CharacterSpriteSet.MapSprite;
                Vector3 spawnPos=Vector3.back;
                do
                {
                    spawnPos = new Vector3(UnityEngine.Random.Range(center.x-width/2,width/2), UnityEngine.Random.Range(center.y-height/2,height/2), 0);

                } while (!pathfinderGraph.data.gridGraph.GetNearest(spawnPos).node.Walkable);

                Debug.Log("Spawn at: " + spawnPos);
                unitGo.transform.position = spawnPos;
                units.Add(unitGo);
                //pathSystem.agents.Add(unitGo.GetComponent<AIPath>());
            }
        }

        public void BackClicked()
        {

            GameSceneController.Instance.LoadWorldMapFromInside();
        }
    }
}
