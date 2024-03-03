using System;
using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public enum StatModifierType
    {
        IncreaseBuffDuration,
        RemoveDebuff
    }

   
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnStatModifier", fileName = "OnStatModifierMixin")]
    public class OnStatModifierSkillMixin:ChanceBasedPassiveSkillMixin
    {
        public StatModifierType type;
       
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            unit.StatusEffectManager.OnStatusEffectAdded += ReactToDebuff;
            // unit.OnLethalDamage += ReactToBeforeDeath;
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.StatusEffectManager.OnStatusEffectAdded -= ReactToDebuff;
        }
        private void ReactToDebuff(Unit unit, BuffDebuffBase buffBase)
        {
            if (DoesActivate(unit, skill.Level))
            {
                if (type == StatModifierType.RemoveDebuff)
                {
                    if(buffBase.BuffData is DebuffData debuff)
                        unit.StatusEffectManager.RemoveBuff(buffBase);
                    // if(buffBase is StatModifier statModifier&& statModifier.HasNegatives()&&!statModifier.HasPositives())
                    //     unit.StatusEffectManager.RemoveStatModifier(statModifier);
                }
                else if (type == StatModifierType.IncreaseBuffDuration)
                {
                    if (buffBase.BuffData is BuffData buff)
                        buffBase.IncreaseCurrentDuration();
                    // if (buffBase is StatModifier statModifier && !statModifier.HasNegatives() &&
                    //     statModifier.HasPositives())
                    //     statModifier.IncreaseCurrentDuration();
                }
            }
        }
       
    }
}