using System.Collections.Generic;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/Resurrect", fileName = "ResurrectMixin")]
    public class RessurectMixin : PassiveSkillMixin
    {

        [SerializeField] private GameObject animation;
        [SerializeField] float[] hpRegPercentage;
        
        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Regenerated HP: ", hpRegPercentage[level]+"%",hpRegPercentage[level+1]+"%"));
            return list;
        }
        public override void BindToUnit(Unit unit, Skill skill)
        {
            base.BindToUnit(unit, skill);
            unit.OnAboutToDie += ReactToBeforeDeath;
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            base.UnbindFromUnit(unit, skill);
            unit.OnAboutToDie -= ReactToBeforeDeath;
        }
        private void ReactToBeforeDeath(Unit unit)
        {
            unit.Hp =(int)(unit.MaxHp*hpRegPercentage[skill.Level]);
        }
      
    }
}