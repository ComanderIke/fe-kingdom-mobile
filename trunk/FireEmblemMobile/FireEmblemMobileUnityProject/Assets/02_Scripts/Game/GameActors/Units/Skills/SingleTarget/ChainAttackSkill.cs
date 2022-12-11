using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public class ChainAttackSkill:SingleTargetSkill
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

        public ChainAttackSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
        }
    }
}