using System.Collections.Generic;
using Game.GameActors.Units.Attributes;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class StatsData
    {
        [SerializeField]
        public List<int> AttackRanges;
        [SerializeField]
        public int Armor;
        [SerializeField]
        public int MaxHp;
        [SerializeField]
        public int MaxSp;
        [SerializeField]
        public int Mov;


        [SerializeField] public Attributes Attributes;
        public StatsData(int maxHp, int maxSp, int mov, Attributes attributes,  int armor,  List<int> attackRanges)
        {
            AttackRanges = attackRanges;
            MaxHp = maxHp;
            MaxSp = maxSp;
            Mov = mov;
            Armor = armor;
            Attributes = attributes;
        }
    }
}