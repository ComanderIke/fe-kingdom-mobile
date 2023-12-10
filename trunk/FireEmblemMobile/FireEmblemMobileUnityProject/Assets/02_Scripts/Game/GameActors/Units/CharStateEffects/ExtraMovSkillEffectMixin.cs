using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
   
    [CreateAssetMenu( menuName = "GameData/Skills/EffectMixin/ExtraMov")]
    public class ExtraMovSkillEffectMixin:UnitTargetSkillEffectMixin
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;

        [SerializeField] private int[] extraMov;

        public override List<EffectDescription> GetEffectDescription(Unit caster, int level)
        {
            string value="+"+extraMov[level];
            string upg = value;
            if(level + 1 <extraMov.Length)
                upg="+"+extraMov[level + 1];
            return new List<EffectDescription>() { new EffectDescription("Mov", value, upg) };
        }

        public override void Activate(Unit target, Unit caster, int level)
        {
            target.Stats.Mov += extraMov[level];
        }

        public override void Deactivate(Unit target, Unit caster, int skillLevel)
        {
            target.Stats.Mov -= extraMov[skillLevel];
        }
    }
}