using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class ActivatedSkill : Skill
    {
        protected ActivatedSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier, string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, tier, upgradeDescr)
        {
        }
    }
}