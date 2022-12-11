using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/IniateCombat", fileName = "IniateCombatSkill")]
    public class OnInitiateCombatSkillBP:PassiveSkillBp
    {
        public override Skill Create()
        {
            return new OnInitiateCombatSkill(Name, Description, Icon, AnimationObject,Cooldown,Tier,UpgradeDescriptions);
        }
    }
}