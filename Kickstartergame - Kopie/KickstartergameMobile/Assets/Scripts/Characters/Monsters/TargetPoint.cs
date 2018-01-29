﻿using System;
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
        public float  ATK_Multiplier { get; set; }
        public int HIT_INFLUENCE { get; set; }
        public float Scale { get; set; }

        public TargetPoint(float x, float y, string name, float attackMultiplier, int hitInfluence, float scale)
        {
            XPos = x;
            YPos = y;
            Name = name;
            ATK_Multiplier = attackMultiplier;
            HIT_INFLUENCE = hitInfluence;
            Scale = scale;
        }
    }
}
