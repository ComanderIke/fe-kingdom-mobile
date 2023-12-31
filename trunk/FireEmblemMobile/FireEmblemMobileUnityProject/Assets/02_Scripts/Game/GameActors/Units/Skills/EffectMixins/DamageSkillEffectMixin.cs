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
        public float[] healPercentageOfDamage;
        public float[] pierceArmorPercentage;
        public bool lethalDmg;
        public bool damageAlly=true;
        [SerializeField] private SkillTransferData skillTransferData;

        public override void Activate(Unit target, Unit caster, int level)
        {


            if(!damageAlly)
                if (!target.Faction.IsOpponentFaction(caster.Faction))
                    return;
           
            Debug.Log("Activate Damage: " + CalculateDamage(caster, target, level) + " " + damageType);
            if (skillTransferData != null)
                MonoUtility.DelayFunction(
                    () => DealDamage(target,caster,level),
                    (float)skillTransferData.data);
            else
                DealDamage(target, caster, level);


        }

        void DealDamage(Unit target, Unit caster, int level)
        {
            if (healPercentageOfDamage.Length != 0)
            {
                int dmg=GetDamageDealtToTarget(caster, target, level);
                caster.Heal((int)(healPercentageOfDamage[level<healPercentageOfDamage.Length?level:0]*dmg));
            }
            Debug.Log("ACTUAL DAMAGE: "+CalculateDamage(caster,target,level));
            target.InflictFixedDamage(caster, CalculateDamage(caster, target, level), damageType);
        }

        public void ShowDamagePreview(Unit target, Unit caster, int level)
        {
            if(!damageAlly)
                if (!target.Faction.IsOpponentFaction(caster.Faction))
                    return;
            Debug.Log("PREVIEW DAMAGE: "+GetDamageDealtToTarget(caster,target,level)+ " To Target: "+target);
            int hpAfter = target.Hp-GetDamageDealtToTarget(caster,target,level);
            if (hpAfter < 0)
                hpAfter = 0;
            Debug.Log("SHOW DAMAGE PREVIEW: "+hpAfter);
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

            

            return GetScaledDamage(caster, target,level);
        }

        private int GetScaledDamage(Unit user, Unit target, int level)
        {
            int baseDamage = dmg[level];
            if (scalingcoeefficient.Length > 0)
            {
                float scaling = scalingcoeefficient[0];
                if (level < scalingcoeefficient.Length)
                    scaling = scalingcoeefficient[level];
                if (scalingType == AttributeType.ATK)
                {
                    baseDamage += (int)(user.BattleComponent.BattleStats.GetDamage()* scaling);
                }
                else
                {
                    baseDamage += (int)(user.Stats.CombinedAttributes().GetAttributeStat(scalingType) *
                                         scaling);
                }

            }
            else
            {
                
            }
            if (target != null && pierceArmorPercentage.Length > 0)
            {
                baseDamage += (int)pierceArmorPercentage[level < pierceArmorPercentage.Length ? level : 0] *
                               target.BattleComponent.BattleStats.GetPhysicalResistance();
            }
            return baseDamage;
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

            list.Add(new EffectDescription("Damage: ", "" + GetScaledDamage(caster, null,level),
                "" + ((level + 1 < dmg.Length) ?GetScaledDamage(caster, null,level+1) : GetScaledDamage(caster,null ,level))));
            if (level < scalingcoeefficient.Length)
                list.Add(new EffectDescription("Scaling " + scalingType + ": ", valueLabel, upgLabel));
            return list;
        }


        public int GetDamageDealtToTarget(Unit caster, Unit target, int level)
        {
            if(!damageAlly)
                if (!target.Faction.IsOpponentFaction(caster.Faction))
                    return 0;
            Debug.Log("DAMAGE DEALT TO: "+target+"TARGET: "+CalculateDamage(caster, target, level));
            return target.GetDamageDealt(caster, CalculateDamage(caster, target, level), damageType);
        }
    }
}

     