using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Debuff
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/EnemyTargetDebuff", fileName = "EnemyDebuff")]
    public class EnemyTargetDebuffSkillBP : SingleTargetSkillBp
    {
        [SerializeField]
        public CharStateEffects.Debuff appliedDebuff;
        public override Skill Create()
        {
            return new EnemyTargetDebuffSkill(Name, Description, Icon, AnimationObject,Tier,UpgradeDescriptions, hpCost,Uses,appliedDebuff);
        }
    }
}