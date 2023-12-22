using System.Collections.Generic;
using Game.GameInput;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public interface IOnCastEffect
    {
        bool ReactToCast(Unit unit);
    }
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnCast", fileName = "OnCast")]
    public class OnCastEffectMixin:ChanceBasedPassiveSkillMixin
    {
     
        public void ReactToCast()
        {
            Debug.Log("Check Activate: "+skill.Name);
            if (DoesActivate((Unit)skill.owner, skill.Level))
            {
                Debug.Log("Activated: "+skill.Name);
                Debug.Log("TODO CLEAR ATTACK EFFECTS AFTER EACH ATTACK?OR IS IT CLEARD ALREADY??");
                foreach (var skilleffect in skillEffectMixins)
                {
                    if (skilleffect is UnitTargetSkillEffectMixin unitTargetSkillEffectMixin)
                    {
                        unitTargetSkillEffectMixin.Activate(skill.owner, skill.owner, skill.Level);
                    }
                    if (skilleffect is SelfTargetSkillEffectMixin selfTargetSkillEffectMixin)
                    {
                        selfTargetSkillEffectMixin.Activate(skill.owner, skill.Level);
                    }
                }

              //  return true;
            }
            else
            {
                Debug.Log("DID NOT ACTIVAGTE CAST SRY MY FRIEND");
            }

           // return false;
        }

        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            unit.SkillManager.OnPayCast += ReactToCast;

        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.SkillManager.OnPayCast -= ReactToCast;
            base.UnbindFromUnit(unit, skill);
            
            
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = base.GetEffectDescription(unit, level);
            return list;
        }
       
    }
}