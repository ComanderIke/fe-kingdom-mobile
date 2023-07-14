using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Passive;
using UnityEngine;
using Random = System.Random;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Miracle", fileName = "MiracleMixin")]
    public class MiracleSkillMixin : ChanceBasedPassiveSkillMixin
    {
        

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            return base.GetEffectDescription(unit, level);
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            unit.OnAboutToDie += ReactToBeforeDeath;
            // unit.OnLethalDamage += ReactToBeforeDeath;
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.OnAboutToDie -= ReactToBeforeDeath;
        }
        private void ReactToBeforeDeath(Unit unit)
        {
            if (DoesActivate(unit, skill.Level))
            {
                unit.Hp = 1;
            }


        }
      
    }
}