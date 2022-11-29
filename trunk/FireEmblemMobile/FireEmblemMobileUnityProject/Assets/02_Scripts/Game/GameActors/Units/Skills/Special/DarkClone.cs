using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Special
{
    [Serializable]

    public class DarkClone : SelfTargetSkill
    {

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public DarkClone(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
        }
    }
}