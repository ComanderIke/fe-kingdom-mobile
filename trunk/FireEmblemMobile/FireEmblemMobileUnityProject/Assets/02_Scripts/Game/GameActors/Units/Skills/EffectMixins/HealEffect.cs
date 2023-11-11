﻿using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Heal", fileName = "HealEffect")]
    public class HealEffect : UnitTargetSkillEffectMixin
    {
        public GameObject healingVfx;
        public float [] heal;
        public AttributeType scalingType;
        public float[] scalingcoeefficient;

        public bool percentage = false;
        public bool healToPercentageHealth = false;
        public override void Activate(Unit target, Unit caster, int level)
        {
      

          
            target.Heal(GetHealAmount(caster,target, level));

            var go = Instantiate(healingVfx, target.GameTransformManager.Transform);
        }

        public int GetHealAmount(Unit caster, Unit target, int level)
        {
            if (healToPercentageHealth)
                return (int)((target.MaxHp * heal[level]) - target.Hp);
            if (percentage)
                return (int)(target.MaxHp*heal[level]);
            if(level<scalingcoeefficient.Length)
                return (int)(heal[level]+(caster.Stats.CombinedAttributes().GetAttributeStat(scalingType) * scalingcoeefficient[level]));
            return (int)(heal[level]);
        }

        public override void Deactivate(Unit user, Unit caster, int skillLevel)
        {
            throw new System.NotImplementedException();
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster, int level)
        {
            var list = new List<EffectDescription>();
            string upgLabelScaling = "";
            string valueLabelScaling = "";
            string valueLabel= (percentage?+(heal[level]*100)+"%":""+GetHealAmount(caster,null,level));
            if (level < scalingcoeefficient.Length)
            {
                valueLabelScaling += scalingcoeefficient[level];
            }
            if (level < heal.Length-1)
            {
                level++;
            }
            if (level < scalingcoeefficient.Length)
            {
                upgLabelScaling += scalingcoeefficient[level];
            }
            string upgLabel=(percentage?+(heal[level]*100)+"%":""+GetHealAmount(caster,null,level));
            
         

            
            
            list.Add( new EffectDescription("Heal: ", valueLabel, upgLabel));
            if(level< scalingcoeefficient.Length)
                list.Add(  new EffectDescription("Scaling " + scalingType + ": ", valueLabelScaling, upgLabelScaling));
            return list;
            
        }


        public void ShowHealPreview(Unit target, Unit caster, int skillLevel)
        {
           
            int hpAfter = target.Hp+GetHealAmount(caster,target,skillLevel);
            if (hpAfter > target.MaxHp)
                hpAfter = target.MaxHp;
            target.visuals.unitRenderer.ShowPreviewHp(hpAfter);
            
        }

        public void HideHealPreview(Unit target)
        {
            target.visuals.unitRenderer.HidePreviewHp();
        }
    }
}