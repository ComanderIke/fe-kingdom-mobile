using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Special
{
    [Serializable]

    public class DarkClone : SelfTargetSkill
    {
        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public DarkClone(string Name, string description, Sprite icon, GameObject animationObject, int cooldown,int tier, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
        }
    }
}