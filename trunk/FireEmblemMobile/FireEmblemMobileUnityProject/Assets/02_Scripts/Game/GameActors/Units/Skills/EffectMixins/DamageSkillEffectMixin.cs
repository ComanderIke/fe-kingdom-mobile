using System.Collections.Generic;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using Game.Mechanics;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Damage", fileName = "DamageSkillEffect")]
    public class DamageSkillEffectMixin : UnitTargetSkillEffectMixin
    {
        public DamageType damageType;
        public int []dmg;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;

        public bool instantDamage = true;
        public override void Activate(Unit target, Unit caster, int level)
        {
            int baseDamageg = dmg[level];

            int scalingdmg = (int)(caster.Stats.CombinedAttributes().GetAttributeStat(scalingType) * scalingcoeefficient[level]);

            if (instantDamage)
            {
                target.InflictFixedDamage(caster, baseDamageg + scalingdmg, damageType);
            }
            else
            {
                caster.Stats.BonusStatsFromEffects.Attack += baseDamageg + scalingdmg;
            }

        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>()
            {
                new EffectDescription("Damage: ", "" + dmg[level],
                    "" + dmg[level + 1])
            };
        }

      
    }
}