using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/SingleCharacterTarget", fileName = "SingleCharacterTarget")]
    public class SingleCharacterTargetSkillBP:SingleTargetSkillBp
    {
        public override Skill Create()
        {
            return new SingleCharacterTargetSkill(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, hpCost, Uses);
        }
    }
}