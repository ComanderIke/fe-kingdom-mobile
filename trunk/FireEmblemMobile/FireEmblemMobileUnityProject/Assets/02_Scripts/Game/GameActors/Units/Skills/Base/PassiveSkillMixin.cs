using System.Collections.Generic;
using LostGrace;
using UnityEngine;
using UnityEngine.WSA;

namespace Game.GameActors.Units.Skills
{
   

    public abstract class PassiveSkillMixin:SkillMixin
    {
       
        [SerializeField]public List<SkillEffectMixin> skillEffectMixins;
        //[SerializeField]private EffectDescription[] effectDescriptionsPerLevel;

        public virtual List<EffectDescription> GetEffectDescription(Unit unit, int level)
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