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

        public bool HasPositives()
        {
            bool positiveAttributes = BonusAttributes[level].AsArray().Any(i => i > 0);
            bool positiveStats = BonusStats[level].AsArray().Any(i => i > 0);
            return positiveAttributes||positiveStats;
        }

        public bool HasNegatives()
        {
            bool negativeAttributes = BonusAttributes[level].AsArray().Any(i => i < 0);
            bool negativeStats = BonusStats[level].AsArray().Any(i => i < 0);
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