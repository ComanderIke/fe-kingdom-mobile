using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [CreateAssetMenu(fileName = "MovementBuff", menuName = "GameData/Buff/Movement")]
    public class MovementBuff:Buff
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;

        [SerializeField] private int[] extraMov;

        public override EffectDescription GetEffectDescription(int level)
        {
            string value="+"+extraMov[level];
            string upg = value;
            if(level + 1 <extraMov.Length)
                upg="+"+extraMov[level + 1];
            return new EffectDescription("Mov", value, upg);
        }
        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster, target, skilllevel);
            target.Stats.Mov += extraMov[level];

        }
        public override void Unapply(Unit target)
        {
           
            target.Stats.Mov -= extraMov[level];
            base.Unapply(target);

        }


    }
}