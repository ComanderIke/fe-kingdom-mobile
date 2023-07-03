using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public class StatModifier : PassiveSkill
    {
        public Attributes BonusAttributes;
        public BonusStats BonusStats;
        public Attributes BonusAttributesPerTier;
        public BonusStats BonusStatsPerTier;
        public StatModifier(string Name, string description, Sprite icon, GameObject animationObject, int tier, string[] upgradeDescr, Attributes bonus, BonusStats bonusStats, Attributes bonusPerTier,BonusStats bonusStatsPerTier ) : base(Name, description, icon, animationObject, tier, upgradeDescr)
        {
            this.BonusAttributes = bonus;
            this.BonusAttributesPerTier = bonusPerTier;
            this.BonusStats = bonusStats;
            this.BonusStatsPerTier = bonusStatsPerTier;
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }

        public override void BindSkill(Unit unit)
        {
            unit.Stats.BonusAttributes.Add(BonusAttributes);
            unit.Stats.BonusStats.Add(BonusStats);
        }
        public override void UnbindSkill(Unit unit)
        {
            unit.Stats.BonusAttributes.Decrease(BonusAttributes);
            unit.Stats.BonusStats.Decrease(BonusStats);
        }
    }
}