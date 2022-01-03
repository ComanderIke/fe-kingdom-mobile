using System.Linq;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Serialization
{
    public class MoveAction:SerializedAction
    {
        private EncounterNode location;
        public MoveAction(EncounterNode location)
        {
            this.location = location;
        }

        public override void PerformAction()
        {
            Debug.Log("Perform Movement Action");
            Player.Instance.Party.EncounterNode = location;
            Player.Instance.Party.GameObject.transform.position = location.gameObject.transform.position;

        }

        public override void Save(SaveData current)
        {
            //Debug.Log("Saving MoveAction");
            // if (party.Faction.IsPlayerControlled)
            // {
            //     current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).locationId = location.UniqueId;
            // }
            // else
            // {
            //     current.campaignData.enemyFactionData.Parties.FirstOrDefault(p => p.name == party.name).locationId = location.UniqueId;
            // }
        }
    }
}