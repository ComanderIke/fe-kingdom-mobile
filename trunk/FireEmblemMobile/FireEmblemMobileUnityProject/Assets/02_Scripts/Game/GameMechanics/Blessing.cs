using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{

    public class Blessing : Skill
    {
        // public Blessing(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr):base(Name, description,icon, animationObject, tier, upgradeDescr)
        // {
        //     SkillType = SkillType.Blessing;
        // }


        public Blessing(string Name, string Description, Sprite icon, int tier, List<PassiveSkillMixin> passiveMixins, ActiveSkillMixin activeMixin) : base(Name, Description, icon, tier, passiveMixins, activeMixin)
        {
        }
    }
}