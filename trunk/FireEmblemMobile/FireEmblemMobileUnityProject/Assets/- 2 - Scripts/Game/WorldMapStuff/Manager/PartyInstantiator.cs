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
           // Debug.Log("Instantiate StartParty: " + startingParty + " at location: " + startPoint);
            var partyGO= Instantiate(partyPrefab, partyParent);
            partyGO.transform.position = startPoint.transform.position;
            partyGO.name = startingParty.Faction.Id == 0 ? "PlayerParty" : "EnemyParty";
            startingParty.GameTransformManager.GameObject = partyGO;
            startPoint.SetActor(startingParty);

            partyGO.GetComponent<WM_ActorController>().actor = startingParty;
            partyGO.GetComponent<WM_ActorRenderer>().actor = startingParty;
    
        }

        public void InstantiateParty(Party spawnParty, WorldMapPosition location)
        {
            //Debug.Log("Instantiate Party: " + spawnParty + " at location: " + location);
            var partyGO= Instantiate(partyPrefab, partyParent);
            partyGO.name = spawnParty.Faction.Id == 0 ? "PlayerParty" : "EnemyParty";
            partyGO.transform.position = location.transform.position;
            spawnParty.GameTransformManager.GameObject = partyGO;
            location.SetActor(spawnParty);
           
            partyGO.GetComponent<WM_ActorController>().actor = spawnParty;
            partyGO.GetComponent<WM_ActorRenderer>().actor = spawnParty;
        
        }
    }
}