using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public interface IBattleEventListener
    {
        
    }
    public interface IAfterCombatListener : IBattleEventListener
    {
        
    }

    public enum BattleEvent
    {
        DuringCombat,
        AfterCombat,
        BeforeCombat,
        InitiateCombat,
        InitiatedOnCombat
    }

    
   
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Combat", fileName = "CombatMixin")]
    public class CombatPassiveMixin:PassiveSkillMixin, IAfterCombatListener//TODO Sub Classes or switch case?
    {

        public BattleEvent CombatState;
        private Unit target;
        private Unit unit;
        private int activatedLevel = -1;
        public void Deactivate()
        {
            
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
            base.UnbindFromUnit(unit, skill);
            unit.BattleComponent.RemoveListener(BattleEvent.AfterCombat, this);
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