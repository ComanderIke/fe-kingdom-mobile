using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using Game.Manager;
using Game.States;
using Game.Systems;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/StartofTurn", fileName = "StartofTurnMixin")]
    public class StartOfTurnPassiveMixin:PassiveSkillMixin
    {
        
        public ConditionBigPackage conditionManager;
       
        public void Deactivate(Unit unit)
        {
           
           
        }
        public void Activate(Unit unit)
        {
            if (conditionManager.Valid(unit))
            {

                foreach (var skillEffect in skillEffectMixins)
                {
                    if (skillEffect is SelfTargetSkillEffectMixin selfTargetSkillMixin)
                        selfTargetSkillMixin.Activate(unit, skill.Level);
                    if (skillEffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                        unitTargetSkillEffectMixin.Activate(unit, unit, skill.Level);
                }
            }

        }

        void StartOfTurn()
        {
            if(GridGameManager.Instance.FactionManager.ActiveFaction.Id==skill.owner.Faction.Id)
                Activate(skill.owner);
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            
            StartOfTurnState.OnStartOfTurnEffects+=StartOfTurn;
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            StartOfTurnState.OnStartOfTurnEffects-=StartOfTurn;
            base.UnbindFromUnit(unit, skill);
           
        }

   
        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
        
            foreach (var skillEffect in skillEffectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(unit,level));
            }
            
            return list;
        }
    }
}