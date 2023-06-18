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
     
        

        public override bool CanTargetCharacters()
        {
            return true;
        }

        protected SingleTargetSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr,hpCost,maxUses)
        {
        }
    }
}