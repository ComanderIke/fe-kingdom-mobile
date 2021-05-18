using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Manager
{
    public class PartyInstantiator : MonoBehaviour
    {
        public WorldMapPosition startPoint;
        public GameObject partyPrefab;
        public Transform partyParent;
        public void InstantiatePartyAtStartPoint(Party startingParty)
        {
            var partyGO= Instantiate(partyPrefab, partyParent);
            partyGO.transform.position = startPoint.transform.position;
            startPoint.Actor = startingParty;
            startingParty.location = startPoint;
            startingParty.GameTransformManager.GameObject = partyGO;
            partyGO.GetComponent<WM_ActorController>().actor = startingParty;
            partyGO.GetComponent<WM_ActorRenderer>().actor = startingParty;
    
        }

        public void InstantiateParty(Party spawnParty, WorldMapPosition location)
        {
            var partyGO= Instantiate(partyPrefab, partyParent);
            partyGO.transform.position = location.transform.position;
            location.Actor = spawnParty;
            spawnParty.location = location;
            spawnParty.GameTransformManager.GameObject = partyGO;
            partyGO.GetComponent<WM_ActorController>().actor = spawnParty;
            partyGO.GetComponent<WM_ActorRenderer>().actor = spawnParty;
        
        }
    }
}