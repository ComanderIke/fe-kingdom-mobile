using System;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/AuraDuringCombat", fileName = "AuraDuringCombat")]
    public class AuraDuringCombatEffectSkillBp:PassiveSkillBp
    {
        public BonusStats BonusStats;
        public int range = 1;
        public override Skill Create()
        {
            return new AuraDuringCombatEffect(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, BonusStats, range);
        }
    }
}