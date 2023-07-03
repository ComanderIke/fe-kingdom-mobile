using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnWaitEffect", fileName = "OnWaitEffect")]
    public class OnWaitEffectSkillBp:PassiveSkillBp
    {
        public override Skill Create()
        {
            return new OnWaitEffectSkill(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions);
        }
    }
}