using System.Linq;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [CreateAssetMenu(fileName = "StatModifierBuff", menuName = "GameData/StatModifier")]
    public class StatModifier : BuffDebuffBase// Special Buff also shown as buffIcon(Arrow up/down) with duration but also shows blue numbers on UI
    {
        public Attributes[] BonusAttributes;
        public BonusStats[] BonusStats;

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
    }
}