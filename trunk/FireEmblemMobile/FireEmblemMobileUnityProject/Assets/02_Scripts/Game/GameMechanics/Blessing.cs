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


        public Blessing(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins, ActiveSkillMixin activeMixin) : base(Name, Description, icon, tier,maxLevel, passiveMixins, activeMixin)
        {
        }

        public override Skill Clone()
        {
            var newBlessing = new Blessing(Name, Description, Icon, Tier, maxLevel,passiveMixins, activeMixin);
            newBlessing.level = Level;
            return newBlessing;
        }
    }
}