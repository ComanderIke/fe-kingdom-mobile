using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/OnBind", fileName = "OnBindMixin")]
    public class OnBindPassiveSkillMixin : PassiveSkillMixin
    {
      
       
        public override void BindToUnit(Unit unit, Skill skill)
        {
           
            foreach(var effect in skillEffectMixins)
            {
                if (effect is SelfTargetSkillEffectMixin stsm)
                {
                    stsm.Activate(unit, skill.level);
                }
            }
            var key = GetBlessing(unit);
            if (key != null)
            {
                foreach (var skillEffect in synergies[key].skillEffectMixins)
                {
                    if (skillEffect is SelfTargetSkillEffectMixin selfTargetSkillMixin)
                        selfTargetSkillMixin.Activate(unit, skill.level);
                }
            }
            base.BindToUnit(unit,skill);
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            
            foreach(var effect in skillEffectMixins)
            {
                if (effect is SelfTargetSkillEffectMixin stsm)
                {
                    stsm.Deactivate(unit, skill.level);
                }
            }
            var key = GetBlessing(unit);
            if (key != null)
            {
                foreach (var skillEffect in synergies[key].skillEffectMixins)
                {
                    if (skillEffect is SelfTargetSkillEffectMixin selfTargetSkillMixin)
                        selfTargetSkillMixin.Deactivate(unit, skill.level);
                }
            }
            base.UnbindFromUnit(unit,skill);

        }
        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            foreach (var effect in skillEffectMixins)
                list.AddRange(effect.GetEffectDescription(unit, level));
            return list;
        }

    }
}