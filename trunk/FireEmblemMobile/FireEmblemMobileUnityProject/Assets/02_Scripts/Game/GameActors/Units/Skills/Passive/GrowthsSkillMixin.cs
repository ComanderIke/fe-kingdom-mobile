using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Growth", fileName = "GrowthMixin")]
    public class GrowthsSkillMixin : PassiveSkillMixin
    {
        [SerializeField] private Attributes[] growths;

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Incr. Growths:", ""+growths[level].MaxHp+"%", ""+growths[level+1].MaxHp+"%"));
            return list;
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            unit.Stats.BonusGrowths.Add(growths[skill.Level]);
           
           
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            if(bound)
                unit.Stats.BonusGrowths.Decrease(growths[skill.Level]);
            base.UnbindFromUnit(unit, skill);
        
        }
    }
}