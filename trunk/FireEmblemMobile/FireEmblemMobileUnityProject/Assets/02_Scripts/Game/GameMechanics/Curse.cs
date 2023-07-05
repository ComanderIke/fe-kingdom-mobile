using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
   

    public class Curse : Skill
    {
        public Curse(string Name, string Description, Sprite icon, int tier) : base(Name, Description, icon, tier)
        {
        }
    }
}