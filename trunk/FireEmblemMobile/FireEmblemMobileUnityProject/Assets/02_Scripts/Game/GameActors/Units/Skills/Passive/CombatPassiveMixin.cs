using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
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
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Combat", fileName = "CombatMixin")]
    public class CombatPassiveMixin:PassiveSkillMixin, IBattleEventListener//TODO Sub Classes or switch case?
    {
        public BattleEvent CombatState;
        
        public void Deactivate(Unit unit, Unit target)
        {
            if (!activated)
                return;
            Debug.Log("DEACTIVATE COMBAT PASSIVE MIXIN"+unit.name+" "+target.name);
            Deactivate(target);
        }
        
        public void Activate(Unit unit, Unit target)
        {
            if (activated)
                return;
            if(toogleAble&& toggledOn||!toogleAble)
                Activate(target);
        }

      

        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.BattleComponent.AddListener(CombatState, this);
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.BattleComponent.RemoveListener(CombatState, this);
            base.UnbindFromUnit(unit, skill);
           
        }
    }
}