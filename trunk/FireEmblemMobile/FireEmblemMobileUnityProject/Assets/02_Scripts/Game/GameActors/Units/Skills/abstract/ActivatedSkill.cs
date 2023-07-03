using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class ActivatedSkill : Skill
    {
        public int currentUses;
        public int maxUses;
        public int hpCost;

        protected ActivatedSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string []upgradeDescr, int hpCost, int maxUses) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
            this.hpCost = hpCost;
            this.maxUses = maxUses;
            this.currentUses = maxUses;
        }
    }
}