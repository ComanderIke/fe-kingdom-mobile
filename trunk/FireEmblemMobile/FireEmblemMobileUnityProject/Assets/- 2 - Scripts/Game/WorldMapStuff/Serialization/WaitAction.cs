using System.Linq;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public class WaitAction:SerializedAction
    {
        private Party party;
        private WM_PartySelectionSystem selectionSystem;
        public WaitAction(Party party, WM_PartySelectionSystem selectionSystem)
        {
            this.party = party;
            this.selectionSystem = selectionSystem;
        }

        public override void PerformAction()
        {
            selectionSystem.DeselectActor();

            party.TurnStateManager.IsWaiting = true;
        }

        public override void Save(SaveData current)
        {
            Debug.Log("Saving MoveAction");
            if (party.Faction.IsPlayerControlled)
            {
                current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).turnStateManager.IsWaiting = party.TurnStateManager.IsWaiting;
            }
            else
            {
                current.campaignData.enemyFactionData.Parties.FirstOrDefault(p => p.name == party.name).turnStateManager.IsWaiting = party.TurnStateManager.IsWaiting;
            }
        }
    }
}