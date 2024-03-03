﻿using System;
using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Battle;
using Game.GameActors.Units;

namespace Game.EncounterAreas.Model
{
    public class SceneTransferData
    {
        private static SceneTransferData _instance;
        public static SceneTransferData Instance
        {
            get { return _instance ??= new SceneTransferData(); }
        }
        
        public BattleMap BattleMap { get; set; }
        public BattleType BattleType { get; set; }
        public string PartyID { get; set; }
        public Party Party{ get; set; }
        public string EnemyPartyID { get; set; }

        public bool TutorialBattle1 { get; set; }

       
        // public LocationData LocationData { get; set; }

     
        public List<Unit> UnitsGoingIntoBattle;
        public BattleOutcome BattleOutCome = BattleOutcome.None;
        public Action BattleFinished;

        public void Reset()
        {
            BattleOutCome = BattleOutcome.None;
            BattleType = BattleType.Normal;
            BattleMap = null;
            UnitsGoingIntoBattle = new List<Unit>();
        }
    }
}