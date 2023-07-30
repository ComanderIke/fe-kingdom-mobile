﻿using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Numbers;
using Game.GameInput;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;
using UnityEngine.WSA;
using Random = System.Random;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnAttack", fileName = "OnAttack")]
    public class OnAttackEffectMixin:ChanceBasedPassiveSkillMixin, IOnAttackEffect
    {
       // [SerializeField] private OnAttackEffect attackEffect;
        // public AttackEffectEnum attackEffect;
        // public string attackEffectExtraDataLabel;
        // public ExtraDataType extraDataType;
        // public float[] attackEffectExtraData;
        
       
        public void ReactToAttack(IBattleActor unit)
        {
            Debug.Log("Check Activate: "+skill.Name);
            if (DoesActivate((Unit)unit, skill.Level))
            {
                Debug.Log("Activated: "+skill.Name);
                Debug.Log("TODO CLEAR ATTACK EFFECTS AFTER EACH ATTACK?OR IS IT CLEARD ALREADY??");
                foreach (var skilleffect in skillEffectMixins)
                {
                    if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                    {
                        unitTargetSkillEffectMixin.Activate((Unit)unit, skill.owner, skill.Level);
                    }
                    if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
                    {
                        selfTargetSkillEffectMixin.Activate(skill.owner, skill.Level);
                    }
                }
            }
        }

        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            Debug.Log("ADD TO ATTACK SKILL LIST: "+skill.Name+ " "+unit.Name);
            unit.BattleComponent.AddToAttackSkillList(skill, this); 
            
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            Debug.Log("REMOVE FROM ATTACK SKILL LIST: "+skill.Name+ " "+unit.Name);
            unit.BattleComponent.RemoveFromAttackSkillList(skill, this); 
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = base.GetEffectDescription(unit, level);
            return list;
        }
       
    }
}