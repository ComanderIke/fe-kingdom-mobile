using System;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/StatModifier", fileName = "StatModifier")]
    public class StatModifierSkillBp:PassiveSkillBp
    {
        public Attributes BonusAttributes;
        public BonusStats BonusStats;
        public Attributes BonusAttributesPerTier;
        public BonusStats BonusStatsPerTier;
        public override Skill Create()
        {
            return new StatModifier(Name, Description, Icon, AnimationObject,Cooldown,Tier,UpgradeDescriptions, BonusAttributes, BonusStats, BonusAttributesPerTier, BonusStatsPerTier);
        }
    }
}