using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]

    public class SingleCharacterTargetSkill:SingleTargetSkill
    {
        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 1;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return true;
        }

        public SingleCharacterTargetSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr,hpCost, maxUses)
        {
        }
    }
}