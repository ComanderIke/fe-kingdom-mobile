using System;
using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    public enum EffectOrigin
    {
        Equipment,
        Effects
    }
   [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/AlwaysActive", fileName = "AlwaysActiveMixin")]
    public class AlwaysActiveSkillMixin : PassiveSkillMixin
    {
        private bool bound = false;
        public override void BindToUnit(Unit unit, Skill skill)
        {
            bound = true;
            base.BindToUnit(unit, skill);
            Debug.Log("Bind Skill "+skill.Name+" "+unit.Name+" "+skill.Level);

            foreach (var skilleffect in skillEffectMixins)
            {
                if (skilleffect is SelfTargetSkillEffectMixin stsm)
                {
                    stsm.Activate(unit, skill.level);
                }
            }
           
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {

            
            if (bound)
            {
                Debug.Log("Unbind Skill " + skill.Name + " " + unit.Name + " " + skill.Level);
                foreach (var skilleffect in skillEffectMixins)
                {
                    if (skilleffect is SelfTargetSkillEffectMixin stsm)
                    {
                        stsm.Deactivate(unit, skill.level);
                    }
                }
            }

            bound = false;
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