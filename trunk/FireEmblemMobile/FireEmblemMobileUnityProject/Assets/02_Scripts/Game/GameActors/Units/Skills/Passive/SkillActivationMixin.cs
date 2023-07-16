using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/SkillActivation", fileName = "SkillActivationMixin")]
    public class SkillActivationMixin : PassiveSkillMixin
    {

      
        [SerializeField] float[] procChanceIncrease;
        
        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Skillactivation:", "+"+procChanceIncrease[level]*100f+"%", "+"+procChanceIncrease[level+1]*100f+"%"));
            return list;
        }
      
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            unit.BonusSkillProcChance += procChanceIncrease[skill.Level];
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.BonusSkillProcChance -= procChanceIncrease[skill.Level];
        }

   
      
    }
}