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


        public Blessing(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins, CombatSkillMixin combatSkillMixin, List<ActiveSkillMixin> activeMixins, SkillTransferData skillTransferData) : base(Name, Description, icon, tier,maxLevel, passiveMixins, combatSkillMixin, activeMixins, skillTransferData)
        {
        }

        public override Skill Clone()
        {
            var newBlessing = new Blessing(Name, Description, Icon, Tier, maxLevel,passiveMixins,CombatSkillMixin, activeMixins, skillTransferData);
            newBlessing.level = Level;
            return newBlessing;
        }
    }
}