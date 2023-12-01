using System.Collections.Generic;
using Game.GameActors.Units.Skills;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public enum BuffType
    {
        CurseResistance,
        MagicResistance,
        Cleansing,
        Regeneration,
        Stealth,
        AbsorbingDmg,
        Invulnerability,
        Stride,
        Rage
        //ClearMovement
        //Add as needed
    }
    [CreateAssetMenu(fileName = "Buff", menuName = "GameData/Buff")]
    public class Buff:BuffDebuffBase
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;
        
        [SerializeField] private BuffType buffType;

        public virtual List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("For "+duration[level]+" Turns: ", "", ""));
            list.Add(new EffectDescription("Grants", buffType.ToString(), buffType.ToString()));
            return list;
        }

        public override int GetHashCode()
        {
            return buffType.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is Buff buff)
            {
                return buff.buffType == buffType;
            }
            return base.Equals(other);
        }

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster, target, skilllevel);
            
        }

        public override void Unapply(Unit target)
        {
            
        }
    }
}