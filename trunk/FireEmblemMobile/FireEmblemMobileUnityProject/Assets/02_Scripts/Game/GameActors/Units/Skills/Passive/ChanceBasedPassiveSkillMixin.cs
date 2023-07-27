using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using LostGrace;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]

    public abstract class ChanceBasedPassiveSkillMixin : PassiveSkillMixin
    {
        public float[] procChance;
        protected float extraChanceFromStatInfluence = 0;
        public AttributeType scalingType;
        public bool DoesActivate(Unit unit, int level)
        {
            return UnityEngine.Random.value <= GetChance(unit, level);
        }

        private float GetChance(Unit unit, int level)
        {
            float chance = procChance[level];
            switch (scalingType)
            {
                case AttributeType.NONE: break;
                case AttributeType.LVL: chance+= unit.ExperienceManager.Level/100f; break;
                default:
                    chance+= unit.Stats.CombinedAttributes().GetAttributeStat(scalingType)/100f;
                break;
            }

            chance += unit.BonusSkillProcChance;
            return chance;
        }
        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = new List<EffectDescription>();
           
            list.Add(new EffectDescription("Chance:", ""+GetChance(unit,level)*100+"%", ""+GetChance(unit,level+1)*100+"%"));
            if(scalingType!=AttributeType.NONE)
                list.Add(new EffectDescription("Scale:", ""+scalingType, ""+scalingType));
            return list;
        }
    }
}