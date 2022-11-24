using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/AfterCombat", fileName = "AfterCombatSkill")]
    public class AfterCombatEffectSkillBP:PassiveSkillBp
    {
        public override Skill Create()
        {
            return new AfterCombatEffectSkill();
        }
    }
}