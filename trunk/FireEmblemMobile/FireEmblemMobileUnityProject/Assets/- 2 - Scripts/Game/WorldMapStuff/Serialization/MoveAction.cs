using System.Linq;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Serialization
{
    public class MoveAction:SerializedAction
    {
        private Party party;
        private LocationController location;
        public MoveAction(Party party, LocationController location)
        {
            this.party = party;
            this.location = location;
        }

        public override void PerformAction()
        {
            Debug.Log("Perform Movement Action from: "+party.location.UniqueId + " to " + location.UniqueId);
            party.TurnStateManager.HasMoved = true;
            party.location.Actor = null;
            party.location.Reset();
         
            party.location = location;
            location.Actor = party;
           
        }

        public override void Save(SaveData current)
        {
            //Debug.Log("Saving MoveAction");
            if (party.Faction.IsPlayerControlled)
            {
                current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).locationId = location.UniqueId;
            }
            else
            {
                current.campaignData.enemyFactionData.Parties.FirstOrDefault(p => p.name == party.name).locationId = location.UniqueId;
            }
        }
    }
}