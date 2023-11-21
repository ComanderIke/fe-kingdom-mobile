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
        public int[] dmg;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;
        public bool lethalDmg;
        [SerializeField] private SkillTransferData skillTransferData;

        public override void Activate(Unit target, Unit caster, int level)
        {


            Debug.Log("Activate Damage: " + CalculateDamage(caster, target, level) + " " + damageType);
            if (skillTransferData != null)
                MonoUtility.DelayFunction(
                    () => target.InflictFixedDamage(caster, CalculateDamage(caster, target, level), damageType),
                    (float)skillTransferData.data);
            else
                target.InflictFixedDamage(caster, CalculateDamage(caster, target, level), damageType);


        }

        public void ShowDamagePreview(Unit target, Unit caster, int level)
        {
            int hpAfter = target.Hp-GetDamageDealtToTarget(caster,target,level);
            if (hpAfter < 0)
                hpAfter = 0;
            target.visuals.unitRenderer.ShowPreviewHp(hpAfter);
        }
        public void HideDamagePreview(Unit target)
        {
            
            target.visuals.unitRenderer.HidePreviewHp();
        }

        public DamageType GetDamageType()
        {
            return damageType;
        }

        public int CalculateDamage(Unit caster, Unit target, int level)
        {
            if (lethalDmg)
            {
                return target.Hp;
            }

            

            return GetScaledDamage(caster, level);
        }

        private int GetScaledDamage(Unit user, int level)
        {
            int baseDamageg = dmg[level];
            if (scalingcoeefficient.Length > 1)
            {
                float scaling = scalingcoeefficient[0];
                if (level < scalingcoeefficient.Length)
                    scaling = scalingcoeefficient[level];
                if (scalingType == AttributeType.ATK)
                {
                    baseDamageg += (int)(user.BattleComponent.BattleStats.GetDamage()* scaling);
                }
                else
                {
                    baseDamageg += (int)(user.Stats.CombinedAttributes().GetAttributeStat(scalingType) *
                                         scaling);
                }

            }
            else
            {
                
            }

            return baseDamageg;
        }
        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster, int level)
        {
            var list = new List<EffectDescription>();
            string upgLabel = "";
            string valueLabel = "";
            if (level < scalingcoeefficient.Length)
            {
                valueLabel += scalingcoeefficient[level];
            }

            if (level + 1 < scalingcoeefficient.Length)
            {
                upgLabel += scalingcoeefficient[level + 1];
            }
            else
            {
                upgLabel = valueLabel;
            }

            list.Add(new EffectDescription("Damage: ", "" + GetScaledDamage(caster, level),
                "" + ((level + 1 < dmg.Length) ?GetScaledDamage(caster, level+1) : GetScaledDamage(caster, level))));
            if (level < scalingcoeefficient.Length)
                list.Add(new EffectDescription("Scaling " + scalingType + ": ", valueLabel, upgLabel));
            return list;
        }


        public int GetDamageDealtToTarget(Unit caster, Unit target, int level)
        {
            return target.GetDamageDealt(caster, CalculateDamage(caster, target, level), damageType);
        }
    }
}

     