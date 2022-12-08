using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
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
      
        // [SerializeField]
        // public int MaxSp;
        [SerializeField]
        public int Mov;
        public event Action OnStatsChanged;

        public Stats()
        {
            BaseAttributes = new Attributes();
            BonusAttributes = new Attributes();
            AttackRanges = new List<int>();
        }

        public Attributes BonusAttributes { get; set; }
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
            stats.BonusAttributes = new Attributes(BonusAttributes);
            stats.Mov = Mov;
           
            return stats;
        }

        public void LoadData(StatsData statsData)
        {
     
            //MaxSp = statsData.MaxSp;
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            Debug.Log("LoadStatsData");
            BaseAttributes = new Attributes(statsData.Attributes);
        }
    }
}