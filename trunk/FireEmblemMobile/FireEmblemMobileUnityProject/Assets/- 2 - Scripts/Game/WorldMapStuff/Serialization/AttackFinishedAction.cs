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
        private BattleOutcome battleOutCome;
        private WM_PartySelectionSystem selectionSystem;
        private string partyId;
        private string enemyId;
        public AttackFinishedAction(Party party, Party enemy,  WM_PartySelectionSystem selectionSystem, BattleOutcome battleOutCome)
        {
            this.party = party;
            this.enemy = enemy;
            this.selectionSystem = selectionSystem;
            this.battleOutCome = battleOutCome;
        }

        public override void PerformAction()
        {
            if (battleOutCome==BattleOutcome.Victory)
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
            else if(battleOutCome==BattleOutcome.Defeat)
            {
                partyId = party.name;
                party.Defeated();
                Debug.Log("Own Party Defeated");
            }
            else
            {
                selectionSystem.DeselectActor();
                Debug.Log("Battle Canceled/Retreated");
            }
        }

        public override void Save(SaveData current)
        {
            Debug.Log("Saving AttackFinished");
            if (party.Faction.IsPlayerControlled)
            {
                if (battleOutCome == BattleOutcome.Victory)
                {
                    current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).SaveData(party);
                    current.campaignData.enemyFactionData.Parties.RemoveAll(p => p.name == enemyId);
                }
                else if (battleOutCome == BattleOutcome.Defeat)
                {
                    current.playerData.factionData.Parties.RemoveAll(p => p.name == partyId);
                }
                else
                {
                    Debug.Log("Save Nothing?");
                }
            }
            else
            {
               Debug.Log("TODO Saving AttackFinishedData when EnemyAI attacked, if that will even be possible");
               //TODO Saving AttackFinishedData when EnemyAI attacked, if that will even be possible
            }
        }
    }
}