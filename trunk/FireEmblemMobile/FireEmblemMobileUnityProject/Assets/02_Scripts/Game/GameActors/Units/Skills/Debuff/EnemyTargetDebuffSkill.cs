using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Debuff
{
    [Serializable]
    public class EnemyTargetDebuffSkill : SingleTargetSkill
    {
        [SerializeField]
        public CharStateEffects.Debuff appliedDebuff;

        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }

      
        public override bool CanTarget(Unit user, Unit target)
        {
            return user.Faction.Id != target.Faction.Id;
        }

        public EnemyTargetDebuffSkill(string Name, string description, Sprite icon, GameObject animationObject, int tier, string[] upgradeDescr,int hpCost, int maxUses, CharStateEffects.Debuff appliedDebuff) : base(Name, description, icon, animationObject, tier, upgradeDescr,hpCost, maxUses)
        {
            this.appliedDebuff = appliedDebuff;
        }
    }
}