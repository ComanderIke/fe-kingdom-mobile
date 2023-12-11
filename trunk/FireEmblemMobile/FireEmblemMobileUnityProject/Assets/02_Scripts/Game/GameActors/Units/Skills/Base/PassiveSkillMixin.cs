using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
   

    public abstract class PassiveSkillMixin:SkillMixin
    {
        [SerializeField] public bool toogleAble;
      
        //[SerializeField]private EffectDescription[] effectDescriptionsPerLevel;

        public virtual List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();

            foreach (var skillEffect in skillEffectMixins)
            {
                list.AddRange(skillEffect.GetEffectDescription(unit, level));
            }
            return list;
        }

    }
}