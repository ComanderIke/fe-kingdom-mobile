using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public abstract class SingleTargetSkill : ActivatedSkill
    {
        public virtual void Activate(Unit user, Unit target)
        {

        }

        public virtual void Effect(Unit user, Unit target)
        {
        }

        public abstract bool CanTarget(Unit user, Unit target);
     
        


        protected SingleTargetSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, tier,upgradeDescr,hpCost,maxUses)
        {
        }
    }
}