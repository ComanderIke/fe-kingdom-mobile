using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SelfTargetSkill : ActivatedSkill
    {
     

        public virtual void Activate(Unit user)
        {

        }

        protected SelfTargetSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, tier,upgradeDescr, hpCost, maxUses)
        {
        }
    }
}