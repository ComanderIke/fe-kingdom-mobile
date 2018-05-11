using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters.Monsters
{
    public class TargetPoint
    {
        public float XPos{ get; set; }
        public float YPos { get; set; }
        public string Name { get; set; }
        public float  DamageMultiplier { get; set; }
        public int HIT_INFLUENCE { get; set; }
        public float Scale { get; set; }
        public bool WeakSpot { get; set; }

        public TargetPoint(float x, float y, string name, float attackMultiplier, int hitInfluence, float scale, bool weakSpot = false)
        {
            XPos = x;
            YPos = y;
            Name = name;
            DamageMultiplier = attackMultiplier;
            HIT_INFLUENCE = hitInfluence;
            Scale = scale;
            WeakSpot = weakSpot;
        }
    }
}
