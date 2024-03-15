using System.Collections.Generic;
using Game.GameActors.Units.Skills.Passive;

namespace Game.States.Mechanics.Battle
{
    public class BonusAttackStats
    {
        public bool BonusAttack { get; set; }
        public Dictionary<AttackEffectEnum, object> AttackEffects { get; set; } //object userData like for Sol healhamount for luna def/res reduction
        public Dictionary<GetHitEffectEnum, object> DefenseEffects { get; set; } 
        public BonusAttackStats()
        {
            BonusAttack = false;
            AttackEffects = new Dictionary<AttackEffectEnum, object>();
            DefenseEffects = new Dictionary<GetHitEffectEnum, object>();
        }

        public void AddAttackEffect(AttackEffectEnum attackEffect, float f)
        {
            if (!AttackEffects.ContainsKey(attackEffect))
            {
                AttackEffects.Add(attackEffect, f);
            }
            else
            {
                if ((float)AttackEffects[attackEffect] < f) //Replace with stronger effect
                    AttackEffects[attackEffect] = f;
            }
        }

        public void AddGetHitEffect(GetHitEffectEnum getHitEffect, float f)
        {
            if (!DefenseEffects.ContainsKey(getHitEffect))
            {
                DefenseEffects.Add(getHitEffect, f);
            }
            else
            {
                if ((float)DefenseEffects[getHitEffect] < f) //Replace with stronger effect
                    DefenseEffects[getHitEffect] = f;
            }
        }
    }
}