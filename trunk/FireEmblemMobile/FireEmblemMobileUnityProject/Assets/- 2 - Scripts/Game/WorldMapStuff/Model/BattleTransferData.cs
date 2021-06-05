﻿using System.Collections.Generic;
using Game.GameActors.Units;

namespace Game.WorldMapStuff.Model
{
    public class BattleTransferData
    {
        private static BattleTransferData _instance;
        public static BattleTransferData Instance
        {
            get { return _instance ??= new BattleTransferData(); }
        }
        
        public List<Unit> EnemyUnits { get; set; }

        public List<Unit> UnitsGoingIntoBattle;
        
    }
}