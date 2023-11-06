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

        public virtual EffectDescription GetEffectDescription(int level)
        {
            return new EffectDescription("Grants", buffType.ToString(), buffType.ToString());
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