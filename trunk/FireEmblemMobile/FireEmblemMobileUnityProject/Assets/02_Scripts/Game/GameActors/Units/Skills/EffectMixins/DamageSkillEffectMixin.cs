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
        public bool damageTypeSameAsAttack;
        public int []dmg;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;
        public bool lethalDmg;
        [SerializeField] private SkillTransferData skillTransferData;

        public override void Activate(Unit target, Unit caster, int level)
        {
            

            Debug.Log("Activate Damage: "+CalculateDamage(caster, target,level)+" "+damageType);
            if(skillTransferData!=null)
                MonoUtility.DelayFunction(()=>target.InflictFixedDamage(caster, CalculateDamage(caster, target,level), damageType), (float)skillTransferData.data);
            else
                target.InflictFixedDamage(caster, CalculateDamage(caster, target,level), damageType);
            

        }

        int CalculateDamage(Unit caster,Unit target, int level)
        {
            if (lethalDmg)
            {
                return target.Hp;
            }
            int baseDamageg = dmg[level];

            if (scalingcoeefficient.Length > level + 1)
            {
                baseDamageg += (int)(caster.Stats.CombinedAttributes().GetAttributeStat(scalingType) *
                                       scalingcoeefficient[level]);
            }

            return baseDamageg ;
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
                    "" + ((level+1<dmg.Length)?dmg[level + 1]:dmg[level])),
                new EffectDescription("Scaling "+scalingType+": ", valueLabel, upgLabel)
            };
        }

      
    }
}