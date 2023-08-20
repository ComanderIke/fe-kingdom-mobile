using System;
using System.Collections.Generic;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [Serializable]
    public enum DebuffType
    {
        Stunned,
        Silenced,
        Frozen, // also makes unit invulnerable for 1 turn(or just against physical?. can be removed differently (fire)
        Disarmed,
        Burned,
        Poisened,
        Snarred,
        Slept,
        Tempted,
        Frog
    }
    [CreateAssetMenu(fileName = "Debuff", menuName = "GameData/Debuff")]
    public class Debuff:BuffDebuffBase
    {
        //[SerializeField]private List<PassiveSkillMixin> buffMixins;
        [SerializeField] public DebuffType debuffType;
        [SerializeField] public List<DebuffType> negateTypes;

        public override void Apply(Unit caster, Unit target, int skilllevel)
        {
            base.Apply(caster, target, skilllevel);
            switch (debuffType)
            {
                case DebuffType.Tempted:
                    target.Faction.RemoveUnit(target);
                    caster.Faction.AddUnit(target);
                    break;
                
            }
        }

        public EffectDescription GetEffectDescription(int level)
        {
            return new EffectDescription("TODO", "TODO", "TODO");
        }
        public override bool TakeEffect(Unit unit)
        {
            switch (debuffType)
            {
                case DebuffType.Frozen: unit.TurnStateManager.UnitTurnFinished(); //unit set phys invulnerable
                    break;
                
            }

            return false;

        }
    }

    
}