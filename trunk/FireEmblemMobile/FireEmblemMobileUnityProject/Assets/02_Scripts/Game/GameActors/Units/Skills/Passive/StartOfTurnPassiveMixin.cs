using System;
using System.Collections.Generic;
using Game.Manager;
using Game.Mechanics;
using LostGrace;
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
                }
            }

        }

        void StartOfTurn()
        {
            Activate(skill.owner);
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            
            ServiceProvider.Instance.GetSystem<TurnSystem>().OnStartTurn+=StartOfTurn;
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            ServiceProvider.Instance.GetSystem<TurnSystem>().OnStartTurn-=StartOfTurn;
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