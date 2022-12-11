using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/AttackModifier", fileName = "AttackModifier")]
    public class AttackModifierSkillBP:PassiveSkillBp
    {
        public override Skill Create()
        {
            return new AttackModifierSkill(Name, Description, Icon, AnimationObject,Cooldown,Tier,UpgradeDescriptions);
        }
    }
}