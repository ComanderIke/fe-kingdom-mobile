using System;
using System.Collections.Generic;
using LostGrace;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]

    public abstract class ChanceBasedPassiveSkillMixin : PassiveSkillMixin
    {
        public float[] procChance;
        public bool DoesActivate(int level)
        {
            return UnityEngine.Random.value <= procChance[level];
        }
        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Chance:", ""+(procChance[level]*100)+"%", ""+(procChance[level+1]*100)+"%"));
            return list;
        }
        protected void OnValidate()
        {
            if (procChance == null||procChance.Length != MAXLEVEL)
            {
                Array.Resize(ref procChance, MAXLEVEL);
            }
        }
    }
}