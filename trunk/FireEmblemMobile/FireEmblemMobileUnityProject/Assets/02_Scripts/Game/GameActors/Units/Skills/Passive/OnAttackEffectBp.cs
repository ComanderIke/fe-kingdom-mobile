using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnAttack", fileName = "OnAttack")]
    public class OnAttackEffectBp:PassiveSkillBp
    {
        [SerializeField] private AttackEffects attackEffects;
        [SerializeField] float procChance;
        
        public override Skill Create()
        {
            return new OnAttackEffect(Name, Description, Icon, AnimationObject, Cooldown, Tier,UpgradeDescriptions, procChance, attackEffects);
        }
    }
}