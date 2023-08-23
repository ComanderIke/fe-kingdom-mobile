using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Passive;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/HpChanged", fileName = "HPChangedSkillMixin")]
    public class HPChangedSkillMixin : PassiveSkillMixin
    {
        private bool bound = false;
        public ConditionBigPackage conditionManager;
        public SkillTransferData SkillTransferData;
        public override void BindToUnit(Unit unit, Skill skill)
        {
            bound = true;
            base.BindToUnit(unit, skill);
            Debug.Log("Bind Skill "+skill.Name+" "+unit.Name+" "+skill.Level);

            unit.HpValueChanged += HpValueChanged;
            
        }

        void HpValueChanged()
        {
            if (conditionManager.Valid(skill.owner))
            {
                SkillTransferData.data = skill.owner.MaxHp - skill.owner.Hp;
                foreach (var skilleffect in skillEffectMixins)
                {
                    if (skilleffect is SelfTargetSkillEffectMixin stsm)
                    {
                        stsm.Activate(skill.owner, skill.level);
                    }
                }

                bound = true;
            }
            else
            {
                SkillTransferData.data = null;
                if (bound)
                {
                    foreach (var skilleffect in skillEffectMixins)
                    {
                        if (skilleffect is SelfTargetSkillEffectMixin stsm)
                        {
                            stsm.Deactivate(skill.owner, skill.level);
                        }
                    }

                }
                
                bound = false;
            }
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.HpValueChanged -= HpValueChanged;
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