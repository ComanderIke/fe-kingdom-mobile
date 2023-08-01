using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.Numbers
{
    [Serializable]
    //[CreateAssetMenu(menuName = "GameData/Unit/Stats", fileName = "UnitStats")]
    public class Stats : ICloneable
    {
        [HideInInspector][SerializeField]
        public List<int> AttackRanges;
        [SerializeField]
        public Attributes BaseAttributes;
        public Attributes BonusAttributesFromEquips { get; set; }
        public Attributes BonusAttributesFromWeapon { get; set; }
        public Attributes BonusAttributesFromEffects { get; set; }
        public BonusStats BonusStatsFromTerrain { get; set; }
        public BonusStats BonusStatsFromEffects { get; set; }
        public BonusStats BonusStatsFromEquips { get; set; }
        public BonusStats BonusStatsFromWeapon { get; set; }
        // [SerializeField]
        // public int MaxSp;
        [SerializeField]
        public int Mov;

        public Stats()
        {
            BaseAttributes = new Attributes();
            BonusAttributesFromEffects = new Attributes();
            BonusAttributesFromEquips = new Attributes();
            BonusAttributesFromWeapon = new Attributes();
            BonusStatsFromEffects = new BonusStats();
            BonusStatsFromEquips = new BonusStats();
            BonusStatsFromTerrain = new BonusStats();
            BonusStatsFromWeapon = new BonusStats();
            AttackRanges = new List<int>();
        }

    
        public Attributes BonusGrowths { get; set; }


        public StatsData GetSaveData()
        {
            return new StatsData(Mov, BaseAttributes,  AttackRanges);
        }
    
        public int GetMaxAttackRange()
        {
            return AttackRanges.Max();
        }

        public object Clone()
        {
            Stats stats = new Stats();
            stats.AttackRanges = new List<int>();
            foreach (int i in AttackRanges)
            {
                stats.AttackRanges.Add(i);
            }

           // stats.MaxSp = MaxSp;
         //  Debug.Log("Clone2Attriubtes");
            stats.BaseAttributes = new Attributes(BaseAttributes);
            stats.BonusAttributesFromEffects = new Attributes(BonusAttributesFromEffects);
            stats.BonusAttributesFromWeapon = new Attributes(BonusAttributesFromWeapon);
            stats.BonusAttributesFromEquips = new Attributes(BonusAttributesFromEquips);
            stats.Mov = Mov;
            stats.BonusStatsFromEffects = new BonusStats(BonusStatsFromEffects);
            stats.BonusStatsFromTerrain = new BonusStats(BonusStatsFromTerrain);
            stats.BonusStatsFromEquips = new BonusStats(BonusStatsFromEquips);
            stats.BonusStatsFromWeapon = new BonusStats(BonusStatsFromWeapon);
           
            return stats;
        }

        public void LoadData(StatsData statsData)
        {
     
            //MaxSp = statsData.MaxSp;
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            BaseAttributes = new Attributes(statsData.Attributes);
        }

        public Attributes CombinedAttributes()
        {
            return BaseAttributes +BonusAttributesFromWeapon+ BonusAttributesFromEffects+ BonusAttributesFromEquips;
        }

        public BonusStats CombinedBonusStats()
        {
            return BonusStatsFromEquips +BonusStatsFromWeapon+ BonusStatsFromEffects+ BonusStatsFromTerrain;
        }

        public Attributes BaseAttributesAndWeapons()
        {
            return BaseAttributes + BonusAttributesFromWeapon;
        }

        public BonusStats GetBonusStatsWithoutWeapon()
        {
            return BonusStatsFromEquips + BonusStatsFromTerrain + BonusStatsFromEffects;
        }
    }
}