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
        
        public BattleMap BattleMap { get; set; }
        public string PartyID { get; set; }
        public Party Party{ get; set; }
        public string EnemyPartyID { get; set; }

        public bool TutorialBattle1 { get; set; }

        public bool IsBoss { get; set; }
        // public LocationData LocationData { get; set; }

     
        public List<Unit> UnitsGoingIntoBattle;
        public BattleOutcome BattleOutCome = BattleOutcome.None;
        public Action BattleFinished;

        public void Reset()
        {
            BattleOutCome = BattleOutcome.None;
            BattleMap = null;
            UnitsGoingIntoBattle = new List<Unit>();
            IsBoss = false;
        }
    }
}