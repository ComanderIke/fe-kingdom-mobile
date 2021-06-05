using System;
using UnityEngine;

namespace Game.GameActors.Units.Attributes
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Unit/Growths", fileName = "UnitGrowths")]
    public class Growths : ScriptableObject, ICloneable
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
        
       


        public object Clone()
        {
            Growths growths = CreateInstance<Growths>();
            growths.Def = Def;
            growths.MaxHp = MaxHp;
            growths.MaxSp = MaxSp;
            growths.Mag = Mag;
            growths.Res = Res;
            growths.Skl = Skl;
            growths.Spd = Spd;
            growths.Str = Str;
            return growths;
        }

        public int[] GetGrowthsArray()
        {
            return new int[] { MaxHp, MaxSp, Str, Mag, Spd, Skl, Def, Res };
        }
    }
}