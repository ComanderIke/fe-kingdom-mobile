using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Players
{
    [System.Serializable]
    public class StatsData
    {
        [SerializeField]
        public List<int> AttackRanges;
  
  
        [SerializeField]
        public int Mov;


        [SerializeField] public Attributes Attributes;
        public StatsData(int mov, Attributes attributes,  List<int> attackRanges)
        {
            AttackRanges = attackRanges;
          
            // MaxSp = maxSp;
            Mov = mov;
            Attributes = attributes;
        }
    }
}