using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Special
{
    [Serializable]

    public class DarkClone : SelfTargetSkill
    {
      

        public DarkClone(string Name, string description, Sprite icon, GameObject animationObject,int tier, string[] upgradeDescr,int hpCost, int maxUses) : base(Name, description, icon, animationObject, tier,upgradeDescr, hpCost,  maxUses)
        {
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }
    }
}