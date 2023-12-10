using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [CreateAssetMenu(fileName = "MovementBuff", menuName = "GameData/Buff/Movement")]
    public class MovementBuff:BuffData
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;

        [SerializeField] private int[] extraMov;

        public override IEnumerable<EffectDescription> GetEffectDescription(int level)
        {
            string value="+"+extraMov[level];
            string upg = value;
            if(level + 1 <extraMov.Length)
                upg="+"+extraMov[level + 1];
            return new List<EffectDescription>() { new EffectDescription("Mov", value, upg) };
        }
        public override void Apply(Unit caster, Unit target, int skillLevel)
        {
            base.Apply(caster, target, skillLevel);
            target.Stats.Mov += extraMov[skillLevel];

        }
        public override void Unapply(Unit target, int skillLevel)
        {
           
            target.Stats.Mov -= extraMov[skillLevel];
            base.Unapply(target, skillLevel);

        }


    }
}