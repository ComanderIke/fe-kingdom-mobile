// using System.Linq;
// using Game.GameActors.Players;
// using Game.Systems;
// using Game.WorldMapStuff.Model;
// using Game.WorldMapStuff.Systems;
// using UnityEngine;
//
// namespace Game.WorldMapStuff.Serialization
// {
//     public class RemovePartyAction:SerializedAction
//     {
//         private Party party;
//
//         private string partyId;
//
//         public RemovePartyAction(Party party)
//         {
//             this.party = party;
//
//         }
//
//         public override void PerformAction()
//         {
//             if (battleOutCome==BattleOutcome.Victory)
//             {
//                 Debug.Log("Enemy Party Defeated");
//                 var location = enemy.location;
//                 enemyId = enemy.name;
//                 enemy.Defeated();
//                 
//                 
//                 SerializedAction action = new MoveAction(party, location);
//                 action.PerformAction();
//                 action.Save(SaveData.currentSaveData);
//                 action = new WaitAction(party, selectionSystem);
//                 action.PerformAction();
//                 action.Save(SaveData.currentSaveData);
//             }
//             else if(battleOutCome==BattleOutcome.Defeat)
//             {
//                 partyId = party.name;
//                 party.Defeated();
//                 Debug.Log("Own Party Defeated");
//             }
//             else
//             {
//                 selectionSystem.DeselectActor();
//                 Debug.Log("Battle Canceled/Retreated");
//             }
//         }
//
//         public override void Save(SaveData current)
//         {
//             Debug.Log("Saving MoveAction");
//             if (party.Faction.IsPlayerControlled)
//             {
//                 if(battleOutCome==BattleOutcome.Victory)
//                     current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).SaveData(party);
//                 else if(battleOutCome==BattleOutcome.Defeat)
//                     current.playerData.factionData.Parties.RemoveAll(p => p.name == partyId);
//                 else
//                 {
//                     Debug.Log("Save Nothing?");
//                 }
//             }
//             else
//             {
//                Debug.Log("TODO Saving AttackFinishedData when EnemyAI attacked, if that will even be possible");
//                //TODO Saving AttackFinishedData when EnemyAI attacked, if that will even be possible
//             }
//         }
//     }
// }