using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [CreateAssetMenu(fileName = "StatModifierBuff", menuName = "GameData/StatModifier")]
    public class StatModifier : BuffDebuffBase// Special Buff also shown as buffIcon(Arrow up/down) with duration but also shows blue numbers on UI
    {
        public Attributes[] BonusAttributes;
        public CombatStats[] BonusStats;

        public override bool Equals(object other)
        {
            if (other is StatModifier statModifier)
            {
                return statModifier.BonusAttributes.Equals(BonusAttributes) && statModifier.BonusStats.Equals(BonusStats);
            }
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return BonusAttributes.GetHashCode();
        }

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster, target, skilllevel);
            if(level < BonusAttributes.Length)
                target.Stats.BonusAttributesFromEffects += BonusAttributes[level]; 
            if(level < BonusStats.Length)
                target.Stats.BonusStatsFromEffects += BonusStats[level];
        }
        public override void Unapply(Unit target)
        {
            if(level < BonusAttributes.Length)
                target.Stats.BonusAttributesFromEffects -= BonusAttributes[level]; 
            if(level < BonusStats.Length)
                target.Stats.BonusStatsFromEffects -= BonusStats[level];
            base.Unapply(target);
            
        }

        public bool HasPositives()
        {
            bool positiveAttributes = false;
            bool positiveStats = false;
            if(level < BonusAttributes.Length)
                positiveAttributes = BonusAttributes[level].AsArray().Any(i => i > 0);
            if(level < BonusStats.Length)
                positiveStats = BonusStats[level].AsArray().Any(i => i > 0);
            return positiveAttributes||positiveStats;
        }

        public bool HasNegatives()
        {
            bool negativeAttributes = false;
            bool negativeStats = false;
            if(level < BonusAttributes.Length)
                negativeAttributes = BonusAttributes[level].AsArray().Any(i => i < 0);
            if(level < BonusStats.Length)
                negativeStats = BonusStats[level].AsArray().Any(i => i < 0);
            return negativeAttributes||negativeStats;
        }

        public List<EffectDescription> GetEffectDescription(int todoLevel)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("For "+duration[level]+" Turns: ", "", ""));
            if (level < BonusAttributes.Length)
            {
                string attributeslabel = BonusAttributes[level].GetTooltipText();
                string valueLabel =BonusAttributes[level].GetTooltipValue();
                // if(level<MAXLEVEL)
                if (level < BonusAttributes.Length - 1)
                    level++;
                
                string upgLabel = BonusAttributes[level].GetTooltipValue();
                list.Add(new EffectDescription(attributeslabel, valueLabel, upgLabel));
            }
            if (level < BonusStats.Length)
            {
                List<string> bonusStatsLabels = BonusStats[level].GetToolTipLabels();
                List<string> bonusStatsValues = BonusStats[level].GetToolTipValues();

                // if(level<MAXLEVEL)
                if (level < BonusStats.Length - 1)
                    level++;

                List<string> bonusStatsUpgrades = BonusStats[level].GetToolTipValues();

                for (int i = 0; i < bonusStatsLabels.Count; i++)
                {
                    list.Add(new EffectDescription(bonusStatsLabels[i], bonusStatsValues[i], bonusStatsUpgrades[i]));
                }
            }

            return list;
           
        }
    }
}