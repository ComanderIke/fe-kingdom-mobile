﻿using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public interface IBattleEventListener
    {
        public void Activate(Unit attacker, Unit defender);
        public void Deactivate(Unit attacker, Unit defender);
    }
   

    public enum BattleEvent
    {
        DuringCombat,
        AfterCombat,
        BeforeCombat,
        InitiateCombat,
        InitiatedOnCombat
    }


    public enum ConditionCompareType
    {
        AND,
        OR,
        XOR
    }
    [Serializable]
    public class ConditionPackage
    {
        public ConditionCompareType CompareType;
        public List<SingleTargetCondition> Conditions;
    }
    [Serializable]
    public class ConditionBigPackage
    {
        public ConditionCompareType CompareType;
        public List<ConditionPackage> Conditions;
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Combat", fileName = "CombatMixin")]
    public class CombatPassiveMixin:PassiveSkillMixin, IBattleEventListener//TODO Sub Classes or switch case?
    {

        public BattleEvent CombatState;
        private Unit target;
        private Unit unit;
        private int activatedLevel = -1;
        private bool activated = false;
        public ConditionBigPackage conditionManager;
       
        public void Deactivate(Unit attacker, Unit defender)
        {
            if (!activated)
                return;
            Debug.Log("DEACTIVATE COMBAT PASSIVE MIXIN");
            foreach (var skillEffect in skillEffectMixins)
            {
                if(skillEffect is SelfTargetSkillEffectMixin selfTargetSkillMixin)
                    selfTargetSkillMixin.Deactivate(unit, activatedLevel);
                if(skillEffect is UnitTargetSkillEffectMixin utm)
                    utm.Deactivate(target, unit, activatedLevel);
            }
            this.unit = null;
            this.target = null;
            activatedLevel = -1;

        }
        public void Activate(Unit unit, Unit target)
        {
            Debug.Log("ACTIVATE COMBAT PASSIVE MIXIN");
            // foreach (var condition in conditions)
            // {
            //     if (!condition.CanTarget(unit, target))
            //         return;
            // }

            activated = true;
            this.unit = unit;
            this.target = target;
            this.activatedLevel = skill.Level;
            foreach (var skillEffect in skillEffectMixins)
            {
                if(skillEffect is SelfTargetSkillEffectMixin selfTargetSkillMixin)
                    selfTargetSkillMixin.Activate(unit, skill.Level);
                if(skillEffect is UnitTargetSkillEffectMixin utm)
                    utm.Activate(unit, target, skill.Level);
            }
           
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.BattleComponent.AddListener(CombatState, this);
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.BattleComponent.RemoveListener(BattleEvent.AfterCombat, this);
            base.UnbindFromUnit(unit, skill);
           
        }

   
          public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
        
            foreach (var skillEffect in skillEffectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(level));
            }
            
            return list;
        }
    }
}