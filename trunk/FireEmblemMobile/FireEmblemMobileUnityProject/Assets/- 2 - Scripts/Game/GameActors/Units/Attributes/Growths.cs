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

        public GrowthsData GetSaveData()
        {
            return new GrowthsData(MaxHp, MaxSp, Str, Mag, Spd, Skl, Def, Res);
        }
        
       


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

        public void LoadData(GrowthsData growthsData)
        {
            Def = growthsData.Def;
            Res = growthsData.Res;
            Spd = growthsData.Spd;
            Skl = growthsData.Skl;
            Mag = growthsData.Mag;
            Str = growthsData.Str;
            MaxHp = growthsData.MaxHp;
            MaxSp = growthsData.MaxSp;
        }
    }
}