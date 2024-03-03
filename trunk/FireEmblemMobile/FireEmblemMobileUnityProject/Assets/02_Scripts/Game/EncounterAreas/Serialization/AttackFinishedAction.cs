using Game.EncounterAreas.Encounters.Battle;
using Game.EncounterAreas.Model;
using Game.SerializedData;

namespace Game.EncounterAreas.Serialization
{
    public class AttackFinishedAction:SerializedAction
    {

       // private EnemyArmyData enemy;
        private BattleOutcome battleOutCome;
        private string enemyId;
        private BattleEncounterController location;
        public AttackFinishedAction(BattleEncounterController location, BattleOutcome battleOutCome)
        {
           // this.enemy = location.enemyArmyData;
            this.location = location;
            this.battleOutCome = battleOutCome;
        }

        public override void PerformAction()
        {
            // if (battleOutCome==BattleOutcome.Victory)
            // {
            //     Debug.Log("VICTORY ATTACK FINISHED ACTION");
            //     enemyId = enemy.name;
            //     enemy.Defeated();
            //     SerializedAction action = new MoveAction(location);
            //     action.PerformAction();
            //     action.Save(SaveData.currentSaveData);
            // }
            // else if(battleOutCome==BattleOutcome.Defeat)
            // {
            //     // partyId = party.name;
            //     // party.Defeated();
            //     Debug.Log("Own Party Defeated");
            // }
            // else
            // {
            //     Debug.Log("Battle Canceled/Retreated");
            // }
        }

        public override void Save(SaveData current)
        {
            // Debug.Log("Saving AttackFinished");
            // if (party.Faction.IsPlayerControlled)
            // {
            //     if (battleOutCome == BattleOutcome.Victory)
            //     {
            //         current.playerData.factionData.Parties.FirstOrDefault(p => p.name == party.name).SaveData(party);
            //         current.campaignData.enemyFactionData.Parties.RemoveAll(p => p.name == enemyId);
            //     }
            //     else if (battleOutCome == BattleOutcome.Defeat)
            //     {
            //         current.playerData.factionData.Parties.RemoveAll(p => p.name == partyId);
            //     }
            //     else
            //     {
            //         Debug.Log("Save Nothing?");
            //     }
            // }
            // else
            // {
            //    Debug.Log("TODO Saving AttackFinishedData when EnemyAI attacked, if that will even be possible");
            //    //TODO Saving AttackFinishedData when EnemyAI attacked, if that will even be possible
            // }
        }
    }
}