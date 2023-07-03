using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [System.Serializable]
    public class SelfTargetBuffSkill:SingleTargetSkill
    {
        [SerializeField]
        public Buff appliedBuff;

        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }


        public override bool CanTarget(Unit user, Unit target)
        {
            return user == target;
        }

        public SelfTargetBuffSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier, string[] upgradeDescr, int hpCost, int maxUses,Buff appliedBuff) : base(Name, description, icon, animationObject, tier, upgradeDescr,hpCost, maxUses)
        {
            this.appliedBuff = appliedBuff;
        }
    }
}