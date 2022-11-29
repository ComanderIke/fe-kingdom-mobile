using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]

    public class SingleCharacterTargetSkill:SingleTargetSkill
    {
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 1;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return true;
        }

        public SingleCharacterTargetSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
        }
    }
}