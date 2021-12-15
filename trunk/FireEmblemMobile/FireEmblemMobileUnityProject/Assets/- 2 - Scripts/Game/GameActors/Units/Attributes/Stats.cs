using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using UnityEngine;

namespace Game.GameActors.Units.Attributes
{
    [Serializable]
    public class Attributes
    {
        [SerializeField]
        public int INT;
        [SerializeField]
        public int CON;
        [SerializeField]
        public int FAITH;
        [SerializeField]
        public int DEX;
        [SerializeField]
        public int AGI;
        [SerializeField]
        public int STR;

        public Attributes(Attributes attributes)
        {
            INT = attributes.INT;
            CON = attributes.CON;
            DEX = attributes.DEX;
            AGI = attributes.AGI;
            STR = attributes.STR;
            FAITH = attributes.FAITH;
        }
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Unit/Stats", fileName = "UnitStats")]
    public class Stats : ScriptableObject, ICloneable
    {
        [SerializeField]
        public List<int> AttackRanges;
        [SerializeField]
        public Attributes Attributes;
        [SerializeField]
        public int Armor;
        [SerializeField]
        public int MaxHp;
        [SerializeField]
        public int MaxSp;
        [SerializeField]
        public int Mov;
      
       
        

        public StatsData GetSaveData()
        {
            return new StatsData(MaxHp, MaxSp, Mov, Attributes, Armor,  AttackRanges);
        }
    
        public int GetMaxAttackRange()
        {
            return AttackRanges.Max();
        }

        public object Clone()
        {
            Stats stats = CreateInstance<Stats>();
            stats.AttackRanges = new List<int>();
            foreach (int i in AttackRanges)
            {
                stats.AttackRanges.Add(i);
            }
            stats.Armor = Armor;
            stats.MaxHp = MaxHp;
            stats.MaxSp = MaxSp;
            stats.Attributes = new Attributes(Attributes);
       
            stats.Mov = Mov;
           
            return stats;
        }

        public void LoadData(StatsData statsData)
        {
            MaxHp = statsData.MaxHp;
            MaxSp = statsData.MaxSp;
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            Armor = statsData.Armor;
            Attributes = new Attributes(statsData.Attributes);
        }
    }
}