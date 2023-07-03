using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public class ChainAttackSkill:SingleTargetSkill
    {
        

        public override bool CanTarget(Unit user, Unit target)
        {
            return true;
        }

        public ChainAttackSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, tier,upgradeDescr,hpCost, maxUses)
        {
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}