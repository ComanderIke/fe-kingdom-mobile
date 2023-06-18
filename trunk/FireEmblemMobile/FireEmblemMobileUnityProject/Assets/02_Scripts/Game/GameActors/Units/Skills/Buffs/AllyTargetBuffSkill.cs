using System;
using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]

    public class AllyTargetBuffSkill : SingleTargetSkill
    {
        [SerializeField]
        public Buff appliedBuff;

        public override List<EffectDescription> GetEffectDescription()
        {
            return null;
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return user.Faction.Id == target.Faction.Id;
        }

        public AllyTargetBuffSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown,int tier, string[] upgradeDescr,int hpCost, int maxUses, Buff appliedBuff) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr,hpCost, maxUses)
        {
            this.appliedBuff = appliedBuff;
        }
    }
}