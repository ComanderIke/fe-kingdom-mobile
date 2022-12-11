using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public class PassiveSkill : Skill
    {
        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }

        public override bool CanTargetCharacters()
        {
            return false;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public PassiveSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
        }
       
    }
}