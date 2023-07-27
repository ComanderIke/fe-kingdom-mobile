using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Numbers;
using Game.GameInput;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;
using Random = System.Random;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnAttack", fileName = "OnAttack")]
    public class OnAttackEffectMixin:ChanceBasedPassiveSkillMixin
    {
       // [SerializeField] private OnAttackEffect attackEffect;
        // public AttackEffectEnum attackEffect;
        // public string attackEffectExtraDataLabel;
        // public ExtraDataType extraDataType;
        // public float[] attackEffectExtraData;
        
       
        private void ReactToAttack(IBattleActor unit)
        {
            if (DoesActivate((Unit)unit, skill.Level))
            {
                foreach (var skilleffect in skillEffectMixins)
                {
                    if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                    {
                        unitTargetSkillEffectMixin.Activate((Unit)unit, skill.owner, skill.Level);
                    }
                }
            }
        }

        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.BattleComponent.onAttack += ReactToAttack;
            
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.BattleComponent.onAttack -= ReactToAttack;
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = base.GetEffectDescription(unit, level);
            return list;
        }
       
    }
}