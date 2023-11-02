using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Canto", fileName = "CantoMixin")]
    public class CantoPassiveSkillMixin : PassiveSkillMixin
    {
      
        [SerializeField] private int[] cantoAmount;
        public override void BindToUnit(Unit unit, Skill skill)
        {
            unit.GridComponent.Canto = cantoAmount[skill.level];
            base.BindToUnit(unit,skill);
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.GridComponent.Canto = 0;
            base.UnbindFromUnit(unit,skill);

        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            return new List<EffectDescription>()
                { new EffectDescription("Amount", ""+cantoAmount[level],  ""+cantoAmount[level+1]) };
        }

    }
}