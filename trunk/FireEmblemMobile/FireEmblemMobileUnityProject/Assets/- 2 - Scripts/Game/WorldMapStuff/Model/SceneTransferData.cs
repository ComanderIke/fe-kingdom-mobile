using System;
using System.Collections.Generic;
using Game.GameActors.Units;

namespace Game.WorldMapStuff.Model
{
    public class SceneTransferData
    {
        private static SceneTransferData _instance;
        public static SceneTransferData Instance
        {
            get { return _instance ??= new SceneTransferData(); }
        }
        
        public List<Unit> EnemyUnits { get; set; }
        public string PartyID { get; set; }
        public string EnemyPartyID { get; set; }
        public string LocationId;

        public List<Unit> UnitsGoingIntoBattle;
        public BattleOutcome BattleOutCome = BattleOutcome.None;
        public Action BattleFinished;

        public void Reset()
        {
            BattleOutCome = BattleOutcome.None;
            EnemyUnits = new List<Unit>();
            UnitsGoingIntoBattle = new List<Unit>();
        }
    }
    [Serializable]
    public enum BattleOutcome
    {
        Victory,
        Defeat,
        None
    }
}