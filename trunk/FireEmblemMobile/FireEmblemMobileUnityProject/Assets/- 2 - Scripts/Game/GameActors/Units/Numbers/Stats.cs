﻿using System;
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
        public Attributes Attributes;
        [HideInInspector][SerializeField]
        public int MaxHp;
        // [SerializeField]
        // public int MaxSp;
        [SerializeField]
        public int Mov;

        public Stats()
        {
            Attributes = new Attributes();
            AttackRanges = new List<int>();
        }
        

        public StatsData GetSaveData()
        {
            return new StatsData(MaxHp, Mov, Attributes,  AttackRanges);
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
            stats.MaxHp = MaxHp;
           // stats.MaxSp = MaxSp;
         //  Debug.Log("Clone2Attriubtes");
            stats.Attributes = new Attributes(Attributes);
       
            stats.Mov = Mov;
           
            return stats;
        }

        public void LoadData(StatsData statsData)
        {
            MaxHp = statsData.MaxHp;
            //MaxSp = statsData.MaxSp;
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            Debug.Log("LoadStatsData");
            Attributes = new Attributes(statsData.Attributes);
        }

        public void Initialize()
        {
            Attributes.OnAttributesUpdated -= AttributesUpdated;
            
            MaxHp = Attributes.CON*Attributes.CON_HP_Mult;
        }

        private void AttributesUpdated()
        {
            MaxHp = Attributes.CON*Attributes.CON_HP_Mult;
        }
    }
}