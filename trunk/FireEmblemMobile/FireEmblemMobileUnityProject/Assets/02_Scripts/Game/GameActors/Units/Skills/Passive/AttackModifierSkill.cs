using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    public class AttackModifierSkill:PassiveSkill
    {
        public AttackModifierSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown,int tier, string[] upgradeDescr) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }
    }
}