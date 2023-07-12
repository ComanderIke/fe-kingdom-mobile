using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
   

    public class Curse : Skill
    {
        public Curse(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins, ActiveSkillMixin activeMixin) : base(Name, Description, icon, tier,maxLevel, passiveMixins, activeMixin)
        {
        }
    }
}