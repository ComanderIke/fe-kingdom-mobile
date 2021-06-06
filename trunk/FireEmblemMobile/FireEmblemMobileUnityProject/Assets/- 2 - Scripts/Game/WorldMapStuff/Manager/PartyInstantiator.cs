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
            partyGO.transform.position = new Vector3(startPoint.transform.position.x,startPoint.transform.position.y,startPoint.transform.position.z-0.1f);
            partyGO.name = startingParty.Faction.Id == 0 ? "PlayerParty" : "EnemyParty";
           
            startingParty.GameTransformManager.GameObject = partyGO;
            startPoint.SetActor(startingParty);

            partyGO.GetComponent<WM_ActorController>().actor = startingParty;
            partyGO.GetComponent<WM_ActorRenderer>().actor = startingParty;
            partyGO.GetComponent<WM_ActorRenderer>().Init();

        }

        public void InstantiateParty(Party spawnParty, WorldMapPosition location)
        {
            Debug.Log("Instantiate Party: " + spawnParty + " at location: " + location);
            var partyGO= Instantiate(partyPrefab, partyParent);
            Debug.Log(spawnParty.Faction);
            Debug.Log(spawnParty.Faction.Id);
            partyGO.name = spawnParty.Faction.Id == 0 ? "PlayerParty" : "EnemyParty";
            partyGO.transform.position =  new Vector3(location.transform.position.x,location.transform.position.y,location.transform.position.z-0.1f);
            spawnParty.GameTransformManager.GameObject = partyGO;
            location.SetActor(spawnParty);
           
            partyGO.GetComponent<WM_ActorController>().actor = spawnParty;
            partyGO.GetComponent<WM_ActorRenderer>().actor = spawnParty;
            partyGO.GetComponent<WM_ActorRenderer>().Init();
        
        }
    }
}