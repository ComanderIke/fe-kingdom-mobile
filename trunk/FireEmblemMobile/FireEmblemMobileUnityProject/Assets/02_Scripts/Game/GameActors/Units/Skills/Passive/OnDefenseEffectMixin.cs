using System.Collections.Generic;
using Game.GameInput;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public interface IOnDefenseEffect
    {
        bool ReactToDefense(IBattleActor unit);
    }
    
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnDefense", fileName = "OnDefense")]
    public class OnDefenseEffectMixin:ChanceBasedPassiveSkillMixin, IOnDefenseEffect
    {
     
        public bool ReactToDefense(IBattleActor unit)
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

                return true;
            }

            return false;
        }

        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            Debug.Log("ADD TO DEFENSE SKILL LIST: "+skill.Name+ " "+unit.Name);
            unit.BattleComponent.AddToDefenseSkillList(skill, this); 
            
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            Debug.Log("REMOVE FROM DEFENSE SKILL LIST: "+skill.Name+ " "+unit.Name);
            unit.BattleComponent.RemoveFromDefenseSkillList(skill, this); 
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = base.GetEffectDescription(unit, level);
            return list;
        }
       
    }
}