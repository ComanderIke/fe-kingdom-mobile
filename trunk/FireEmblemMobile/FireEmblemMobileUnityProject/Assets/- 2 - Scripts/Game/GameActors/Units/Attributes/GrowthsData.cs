using System;
using UnityEngine;

namespace Game.GameActors.Units.Attributes
{
    [System.Serializable]
    public class GrowthsData
    {
        [SerializeField]
        public int MaxHp;
        [SerializeField]
        public int MaxSp;
        [SerializeField]
        public int Str;
        [SerializeField]
        public int Mag;
        [SerializeField]
        public int Spd;
        [SerializeField]
        public int Skl;
        [SerializeField]
        public int Def;
        [SerializeField]
        public int Res;
        public GrowthsData(int maxHp, int maxSp, int str, int mag, int spd, int skl, int def, int res)
        {
            this.MaxHp = maxHp;
            this.MaxSp = maxSp;
            this.Str = str;
            this.Mag = mag;
            this.Spd = spd;
            this.Def = def;
            this.Res = res;
            this.Skl = skl;
        }
    }
}