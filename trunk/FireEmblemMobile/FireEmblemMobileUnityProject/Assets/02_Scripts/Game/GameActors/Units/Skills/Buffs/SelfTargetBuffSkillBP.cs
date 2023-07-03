using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/SelfTargetBuff", fileName = "SelfTargetBuff")]
    public class SelfTargetBuffSkillBP:SingleTargetSkillBp
    {
        [SerializeField]
        public Buff appliedBuff;
        
        public override Skill Create()
        {
            return new SelfTargetBuffSkill(Name, Description, Icon, AnimationObject,Tier, UpgradeDescriptions, hpCost, Uses,appliedBuff);
        }
    }
}