using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Custom", fileName = "CustomMixin")]
    public class CustomPassiveSkillMixin:PassiveSkillMixin, ITurnStateListener
    {
        
        public override void BindToUnit(Unit unit, Skill skill)
        {
            //skill.SubscribeTo(unit.BattleComponent.onAttack);
            base.BindToUnit(unit, skill);
            foreach (var skillEffect in skillEffectMixins)
            {
                if (skillEffect is SelfTargetSkillEffectMixin stsem)
                {
                    stsem.Activate(unit, skill.Level);
                }
                if (skillEffect is UnitTargetSkillEffectMixin utsem)
                {
                    utsem.Activate(unit, unit, skill.Level);
                }
            }

        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            foreach (var skillEffect in skillEffectMixins)
            {
                if (skillEffect is SelfTargetSkillEffectMixin stsem)
                {
                    stsem.Deactivate(unit, skill.Level);
                }
                if (skillEffect is UnitTargetSkillEffectMixin utsem)
                {
                    utsem.Deactivate(unit, unit, skill.Level);
                }
            }
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