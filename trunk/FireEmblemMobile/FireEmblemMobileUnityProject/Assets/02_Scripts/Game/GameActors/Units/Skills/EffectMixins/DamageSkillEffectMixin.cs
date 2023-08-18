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

        public override void Activate(Unit target, Unit caster, int level)
        {
            

            
            target.InflictFixedDamage(caster, CalculateDamage(caster, level), damageType);
            

        }

        int CalculateDamage(Unit caster, int level)
        {
            int baseDamageg = dmg[level];

            int scalingdmg = (int)(caster.Stats.CombinedAttributes().GetAttributeStat(scalingType) * scalingcoeefficient[level]);
            return baseDamageg + scalingdmg;
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            string upgLabel = "";
            string valueLabel = "";
            if (level < scalingcoeefficient.Length)
            {
                valueLabel += scalingcoeefficient[level];
            }
            if (level+1 < scalingcoeefficient.Length)
            {
                upgLabel += scalingcoeefficient[level + 1];
            }
            else
            {
                upgLabel = valueLabel;
            }

            return new List<EffectDescription>()
            {
                new EffectDescription("Damage: ", "" + dmg[level],
                    "" + dmg[level + 1]),
                new EffectDescription("Scaling "+scalingType+": ", valueLabel, upgLabel)
            };
        }

      
    }
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/BattleModifier", fileName = "BattleModifierSkillEffect")]
    public class battleModifierSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public bool excessHitToCrit = false;

        public override void Activate(Unit user, int level)
        {
            user.BattleComponent.BattleStats.ExcessHitToCrit = excessHitToCrit;
        }
        

        public override void Deactivate(Unit user, int skillLevel)
        {
            user.BattleComponent.BattleStats.ExcessHitToCrit = false;
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return null;
        }

      
    }
}