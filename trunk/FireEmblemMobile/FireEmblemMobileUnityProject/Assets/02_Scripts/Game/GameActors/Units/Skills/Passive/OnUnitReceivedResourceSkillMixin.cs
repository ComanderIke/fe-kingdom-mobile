using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public enum ReceivedResourceType
    {
        Exp,
        Healing
    }
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/ExpMixin", fileName = "ExpSkillMixin")]
    public class OnUnitReceivedResourceSkillMixin : PassiveSkillMixin
    {
        [SerializeField]private ReceivedResourceType type;
        [SerializeField] private float[] multiplicator;
        [SerializeField] private string effectLabel = "Exp:";
        public override void BindToUnit(Unit unit, Skill skill)
        {
            switch (type)
            {
                case ReceivedResourceType.Exp:  unit.ExperienceManager.ExpMultipliers.Add(multiplicator[skill.Level]);break;
                case ReceivedResourceType.Healing: unit.HealingMultipliers.Add(multiplicator[skill.Level]);
                    break;
            }
           
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            switch (type)
            {
                case ReceivedResourceType.Exp:  unit.ExperienceManager.ExpMultipliers.Remove(multiplicator[skill.Level]);break;
                case ReceivedResourceType.Healing: unit.HealingMultipliers.Remove(multiplicator[skill.Level]);
                    break;
            }
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            if(level<MAXLEVEL)
                return new List<EffectDescription>()
                { new EffectDescription(effectLabel, "+" + ((multiplicator[level] * 100) - 100) + "%",  + ((multiplicator[level+1] * 100) - 100) + "%") };
            else
            {
                return new List<EffectDescription>()
                    { new EffectDescription(effectLabel, "+" + ((multiplicator[level] * 100) - 100) + "%",  "max") };
            }
        }
        void OnValidate()
        {
            if (multiplicator == null||multiplicator.Length != MAXLEVEL)
            {
                Array.Resize(ref multiplicator, MAXLEVEL);
            }
        }

    }
}