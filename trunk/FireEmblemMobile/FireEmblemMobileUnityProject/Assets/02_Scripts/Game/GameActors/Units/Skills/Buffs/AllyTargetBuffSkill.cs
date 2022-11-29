using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]

    public class AllyTargetBuffSkill : SingleTargetSkill
    {
        [SerializeField]
        public Buff appliedBuff;
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return user.Faction.Id == target.Faction.Id;
        }

        public AllyTargetBuffSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, Buff appliedBuff) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.appliedBuff = appliedBuff;
        }
    }
}