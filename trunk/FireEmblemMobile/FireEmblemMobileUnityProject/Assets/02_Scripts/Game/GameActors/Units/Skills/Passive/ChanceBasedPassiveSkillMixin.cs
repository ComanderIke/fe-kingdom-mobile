using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]

    public abstract class ChanceBasedPassiveSkillMixin : PassiveSkillMixin
    {
        public float[] procChance;
        public bool skillTransferDataIsMultiplier;
        protected float extraChanceFromStatInfluence = 0;
        public SkillTransferData SkillTransferData;
        public AttributeType scalingType;
        public bool DoesActivate(Unit unit, int level)
        {
            Debug.Log("Chance: "+GetChance(unit, level));
            return UnityEngine.Random.value <= GetChance(unit, level);
        }

        private float GetChance(Unit unit, int level)
        {
            float chance = procChance.Length>level?procChance[level]:procChance[0];
            if (skillTransferDataIsMultiplier && SkillTransferData.data != null)
                chance *= (float)SkillTransferData.data;
            switch (scalingType)
            {
                case AttributeType.NONE: break;
                case AttributeType.LVL: chance+= unit.ExperienceManager.Level/100f; break;
                default:
                    chance+= unit.Stats.CombinedAttributes().GetAttributeStat(scalingType)/100f;
                break;
            }

            chance += unit.BonusSkillProcChance;
            chance += (Player.Instance.Modifiers.SkillActivation-1);
            return chance;
        }
        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            var list = base.GetEffectDescription(unit, level);
           
            list.Add(new EffectDescription("Chance:", ""+GetChance(unit,level)*100+"%", ""+GetChance(unit,level+1)*100+"%"));
            if(scalingType!=AttributeType.NONE)
                list.Add(new EffectDescription("Scale:", ""+scalingType, ""+scalingType));
            return list;
        }
    }
}