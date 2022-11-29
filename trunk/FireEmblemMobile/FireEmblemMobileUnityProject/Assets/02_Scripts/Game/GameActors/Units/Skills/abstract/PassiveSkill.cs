using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public class PassiveSkill : Skill
    {
        public override bool CanTargetCharacters()
        {
            return false;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public PassiveSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
        }
    }
}