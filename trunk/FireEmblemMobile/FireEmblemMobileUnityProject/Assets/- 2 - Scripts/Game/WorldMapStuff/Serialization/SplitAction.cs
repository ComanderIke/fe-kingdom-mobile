using System.Linq;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Manager;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    internal class SplitAction:SerializedAction
    {
        private Party party;
        private PartyInstantiator partyInstantiator;

        private Party splitParty;
        //private string otherPartyId;
        public SplitAction(Party party, PartyInstantiator partyInstantiator)
        {
            this.party = party;
            this.partyInstantiator = partyInstantiator;
            //otherPartyId = otherParty.name;
        }

        public override void PerformAction()
        {
            splitParty=party.Split();
            partyInstantiator.InstantiateParty(splitParty, party.location.worldMapPosition);
            splitParty.GameTransformManager.SetInputReceiver(party.GameTransformManager.GetInputReceiver());
        }

        public override void Save(SaveData current)
        {
   
            if (party.Faction.IsPlayerControlled)
            {
                current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).SaveData(party);
                Debug.Log(party.location.name+" "+party.location.worldMapPosition.name);
                Debug.Log(splitParty.location.name+" "+splitParty.location.worldMapPosition.name);
                current.playerData.factionData.Parties.Add(new PartyData(splitParty));
            }
        }
    }
}