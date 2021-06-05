using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using UnityEngine;

namespace Game.GameActors.Units.Attributes
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Unit/Stats", fileName = "UnitStats")]
    public class Stats : ScriptableObject, ICloneable
    {
        [SerializeField]
        public List<int> AttackRanges;
        [SerializeField]
        public int Def;
        [SerializeField]
        public int Mag;
        [SerializeField]
        public int MaxHp;
        [SerializeField]
        public int MaxSp;
        [SerializeField]
        public int Mov;
        [SerializeField]
        public int Res;
        [SerializeField]
        public int Skl;
        [SerializeField]
        public int Spd;
        [SerializeField]
        public int Str;

        public StatsData GetSaveData()
        {
            return new StatsData(MaxHp, MaxSp, Mov, Res, Skl, Spd, Str, Def, Mag, AttackRanges);
        }
        public int[] GetStatArray()
        {
            return new int[] { MaxHp, MaxSp, Str, Mag, Skl, Spd, Def, Res };
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
            stats.Def = Def;
            stats.MaxHp = MaxHp;
            stats.MaxSp = MaxSp;
            stats.Mag = Mag;
            stats.Mov = Mov;
            stats.Res = Res;
            stats.Skl = Skl;
            stats.Spd = Spd;
            stats.Str = Str;
            return stats;
        }

        public void LoadData(StatsData statsData)
        {
            MaxHp = statsData.MaxHp;
            MaxSp = statsData.MaxSp;
            Mov = statsData.Mov;
            AttackRanges = statsData.AttackRanges;
            Str = statsData.Str;
            Mag = statsData.Mag;
            Def = statsData.Def;
            Res = statsData.Res;
            Spd = statsData.Spd;
            Skl = statsData.Skl;
        }
    }
}