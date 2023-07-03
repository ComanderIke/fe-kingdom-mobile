using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]

    public class SingleCharacterTargetSkill:SingleTargetSkill
    {
       

        public override bool CanTarget(Unit user, Unit target)
        {
            return true;
        }

        public SingleCharacterTargetSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, tier,upgradeDescr,hpCost, maxUses)
        {
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }
    }
}