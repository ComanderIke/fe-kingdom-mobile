using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Immunity", fileName = "Immunity")]
    public class ImmunitySkillBP:PassiveSkillBp
    {
        [SerializeField]private ImmunityType type;
        public override Skill Create()
        {
            return new Immunity(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, type);
        }
    }
}