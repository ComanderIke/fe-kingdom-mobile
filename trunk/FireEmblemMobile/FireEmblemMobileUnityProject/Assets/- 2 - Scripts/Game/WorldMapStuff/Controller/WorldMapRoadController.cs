using System;
using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    [ExecuteInEditMode]
    public class WorldMapRoadController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private WorldMapPosition StartPosition;
        [SerializeField] private GameObject RoadPrefab;
        [SerializeField] private Transform RoadParent;
  
        

        private void OnEnable()
        {
            Debug.Log("Create Roads");
            foreach (Transform child in RoadParent.GetComponentsInChildren<Transform>()) {
                if(child!=RoadParent)
                    GameObject.DestroyImmediate(child.gameObject);
            }
            StartPosition.CreateRoads(RoadPrefab, RoadParent);
            WM_PartySelectionSystem.OnDeselectParty += UpdateAllLocations;
        }

        private void OnDisable()
        {
            WM_PartySelectionSystem.OnDeselectParty -= UpdateAllLocations;
        }

        private void UpdateAllLocations()
        {
            Debug.Log("UpdateAllLocations!");
            foreach (var pos in GetComponentsInChildren<WorldMapPosition>())
            {
                pos.UpdateActiveLocations();
                pos.UpdatePositionLocations();
                pos.HideInteractableConnections();
                
            }
               
        }

        private void Update()
        {
            StartPosition.DrawRoads();
        }

     
    }
}
