using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/AllyTargetBuff", fileName = "AllyTargetBuff")]
    public class AllyTargetBuffSkillBP : SingleTargetSkillBp
    {
        [SerializeField]
        public Buff appliedBuff;
        public override Skill Create()
        {
            return new AllyTargetBuffSkill();
        }
    
    }
}