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
            base.BindToUnit(unit,skill);
            Activate(unit);
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {

            Deactivate(unit);
            base.UnbindFromUnit(unit,skill);

        }
       

    }
}