using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class PassiveSkillMixin:SkillMixin
    {
        
        //[SerializeField]private EffectDescription[] effectDescriptionsPerLevel;

        public abstract List<EffectDescription> GetEffectDescription(Unit unit, int level);

    }
}