﻿using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Debuff
{
    [Serializable]
    public class EnemyTargetDebuffSkill : SingleTargetSkill
    {
        [SerializeField]
        public CharStateEffects.Debuff appliedDebuff;
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return user.Faction.Id != target.Faction.Id;
        }

        public EnemyTargetDebuffSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, CharStateEffects.Debuff appliedDebuff) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.appliedDebuff = appliedDebuff;
        }
    }
}