using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class StatsData
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
        public StatsData(int maxHp, int maxSp, int mov, int res, int skl, int spd, int str, int def, int mag, List<int> attackRanges)
        {
            AttackRanges = attackRanges;
            MaxHp = maxHp;
            MaxSp = maxSp;
            Mov = mov;
            Str = str;
            Skl = skl;
            Spd = spd;
            Def = def;
            Res = res;
            Mag = mag;
        }
    }
}