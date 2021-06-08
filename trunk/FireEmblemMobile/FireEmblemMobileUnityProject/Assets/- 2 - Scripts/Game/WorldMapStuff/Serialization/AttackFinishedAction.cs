using System.Linq;
using Game.GameActors.Players;
using Game.Systems;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;
using UnityEngine;

namespace Game.WorldMapStuff.Serialization
{
    public class AttackFinishedAction:SerializedAction
    {
        private Party party;
        private Party enemy;
        private bool victory;
        private WM_PartySelectionSystem selectionSystem;
        private string partyId;
        private string enemyId;
        public AttackFinishedAction(Party party, Party enemy,  WM_PartySelectionSystem selectionSystem, bool victory)
        {
            this.party = party;
            this.enemy = enemy;
            this.selectionSystem = selectionSystem;
            this.victory = victory;
        }

        public override void PerformAction()
        {
            if (victory)
            {
                Debug.Log("Enemy Party Defeated");
                var location = enemy.location;
                enemyId = enemy.name;
                enemy.Defeated();
                
                
                SerializedAction action = new MoveAction(party, location);
                action.PerformAction();
                action.Save(SaveData.currentSaveData);
                action = new WaitAction(party, selectionSystem);
                action.PerformAction();
                action.Save(SaveData.currentSaveData);
            }
            else
            {
                partyId = party.name;
                party.Defeated();
                Debug.Log("Own Party Defeated");
            }
        }

        public override void Save(SaveData current)
        {
            Debug.Log("Saving MoveAction");
            if (party.Faction.IsPlayerControlled)
            {
                if(victory)
                    current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).SaveData(party);
                else
                    current.playerData.factionData.Parties.RemoveAll(p => p.name == partyId);
            }
            else
            {
                if(victory)
                    current.campaignData.enemyFactionData.Parties.RemoveAll(p => p.name == enemyId);
                else
                    current.campaignData.enemyFactionData.Parties.FirstOrDefault(p => p.name == enemy.name).SaveData(enemy);
            }
        }
    }
}