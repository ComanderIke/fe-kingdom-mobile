using System.Linq;
using Game.Systems;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    internal class JoinAction:SerializedAction
    {
        private Party party;
        private Party otherParty;
        private string otherPartyId;
        public JoinAction(Party party, Party otherParty)
        {
            this.party = party;
            this.otherParty = otherParty;
            otherPartyId = otherParty.name;
        }

        public override void PerformAction()
        {
            party.Join(otherParty);
        }

        public override void Save(SaveData current)
        {
            Debug.Log("Saving MoveAction");
            if (party.Faction.IsPlayerControlled)
            {
                current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).SaveData(party);
                current.playerData.factionData.Parties.RemoveAll(p => p.name == otherPartyId);

            }
        }
    }
}