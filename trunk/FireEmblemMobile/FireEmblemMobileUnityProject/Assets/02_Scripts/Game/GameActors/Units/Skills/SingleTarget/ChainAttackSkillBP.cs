using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/ChainAttack", fileName = "ChainAttack")]
    public class ChainAttackSkillBP:SingleTargetSkillBp
    {
        public override Skill Create()
        {
            return new ChainAttackSkill(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, hpCost, Uses);
        }
    }
}