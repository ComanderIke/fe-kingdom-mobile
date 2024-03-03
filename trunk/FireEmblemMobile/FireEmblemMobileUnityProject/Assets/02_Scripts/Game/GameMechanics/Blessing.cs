using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.EffectMixins;
using Game.GUI;
using UnityEngine;

namespace Game.GameMechanics
{

    public class Blessing : Skill
    {
        // public Blessing(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr):base(Name, description,icon, animationObject, tier, upgradeDescr)
        // {
        //     SkillType = SkillType.Blessing;
        // }


        public Blessing(string Name, string Description, Sprite icon, int tier,int maxLevel, List<PassiveSkillMixin> passiveMixins, CombatSkillMixin combatSkillMixin, List<ActiveSkillMixin> activeMixins, SkillTransferData skillTransferData,  God god) : base(Name, Description, icon, tier,maxLevel, passiveMixins, combatSkillMixin, activeMixins, skillTransferData)
        {
            God = god;
        }

        public God God { get; set; }
        

        public override Skill Clone()
        {
            var newBlessing = new Blessing(Name, Description, Icon, Tier, maxLevel,passiveMixins,CombatSkillMixin, activeMixins, skillTransferData, God);
            newBlessing.level = Level;
            return newBlessing;
        }
    }
}