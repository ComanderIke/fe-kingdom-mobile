using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameActors.Units.Attributes
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Unit/Stats", fileName = "UnitStats")]
    public class Stats : ScriptableObject
    {
        public List<int> AttackRanges;
        public int Def;
        public int Mag;
        public int MaxHp;
        public int MaxSp;
        public int Mov;
        public int Res;
        public int Skl;
        public int Spd;
        public int Str;

        public int GetMaxAttackRange()
        {
            return AttackRanges.Max();
        }
    }
}