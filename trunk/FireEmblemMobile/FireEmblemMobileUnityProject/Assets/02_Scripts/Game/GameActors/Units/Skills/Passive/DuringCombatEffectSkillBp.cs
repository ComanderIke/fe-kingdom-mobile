using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/DuringCombat", fileName = "DuringCombatSkill")]
    public class DuringCombatEffectSkillBp:PassiveSkillBp
    {
        public override Skill Create()
        {
            return new DuringCombatEffect(Name, Description, Icon, AnimationObject,Cooldown,UpgradeDescriptions);
        }
    }
}