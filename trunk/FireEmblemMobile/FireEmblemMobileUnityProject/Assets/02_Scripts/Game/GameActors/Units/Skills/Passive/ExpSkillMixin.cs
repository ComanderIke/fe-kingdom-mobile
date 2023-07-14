using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/ExpMixin", fileName = "ExpSkillMixin")]
    public class ExpSkillMixin : PassiveSkillMixin
    {
        [SerializeField] private float[] expMul;
        [SerializeField] private string effectLabel = "Exp Gained:";
        public override void BindToUnit(Unit unit, Skill skill)
        {
            unit.ExperienceManager.ExpMultipliers.Add(expMul[skill.Level]);
        }
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {
            unit.ExperienceManager.ExpMultipliers.Remove(expMul[skill.Level]);
        }

        public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
        {
            if(level<MAXLEVEL)
                return new List<EffectDescription>()
                { new EffectDescription(effectLabel, "+" + ((expMul[level] * 100) - 100) + "%",  + ((expMul[level+1] * 100) - 100) + "%") };
            else
            {
                return new List<EffectDescription>()
                    { new EffectDescription(effectLabel, "+" + ((expMul[level] * 100) - 100) + "%",  "max") };
            }
        }
        void OnValidate()
        {
            if (expMul == null||expMul.Length != MAXLEVEL)
            {
                Array.Resize(ref expMul, MAXLEVEL);
            }
        }

    }
}