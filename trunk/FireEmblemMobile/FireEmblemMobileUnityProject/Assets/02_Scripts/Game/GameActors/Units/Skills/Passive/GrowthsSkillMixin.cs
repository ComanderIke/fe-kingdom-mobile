using System.Collections.Generic;
using System.Runtime.InteropServices;
using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Growth", fileName = "GrowthMixin")]
    public class GrowthsSkillMixin : PassiveSkillMixin
    {
        [SerializeField] private Attributes[] growths;

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Increased Growths:", ""+growths[level].CON+"%", ""+growths[level+1].CON+"%"));
            return list;
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            unit.Stats.BonusGrowths.Add(growths[skill.Level]);
        }

        

        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.Stats.BonusGrowths.Decrease(growths[skill.Level]);
        }
    }
}