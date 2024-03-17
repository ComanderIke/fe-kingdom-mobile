using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.EffectMixins;
using Game.States.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Base
{
    public enum BonusEffectType
    {
        Effect,
        Blessing,
        Relic,
        Curse,
        Gem
    }
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Stats", fileName = "StatsEffect")]
    public class BoostStatsEffect : SelfTargetSkillEffectMixin
    {
      
        public Attributes[] BonusAttributes;
        public CombatStats[] BonusStats;
        public bool attributesIsGrowths;
        public BonusEffectType effectType;
        public int BonusMov; 
        [SerializeField] private int[] cantoAmount;
        public float multiplier = 1;
        public bool skillTransferDataIsMultiplier;
        public float skillTransferDataMultiplierMultiplier;
     
        public SkillTransferData SkillTransferData;
        private bool activated = false;
        private float activatedMultiplier = 1;
        private Unit target;
        private int activatedLevel = 0;
        
        public override void Activate(Unit target, int level)
        {
MyDebug.LogTest("ACTIVATE BOOST STATS EFFECT");
            if (activated)
                return;
            this.target = target;
            this.activatedLevel = level;
 
                
            ApplyBonuses(level);
            activated = true;
        }

        public override void Deactivate(Unit target, int level)
        {
            if (!activated)
                return;
            RemoveBonuses(level);
            this.target = null;
            this.activatedLevel = 0;
           
            activated = false;
           
        }
        private void ApplyBonuses(int level)
        {
            if (skillTransferDataIsMultiplier && SkillTransferData != null&& SkillTransferData.data!=null)
            {
                multiplier = (float)SkillTransferData.data * skillTransferDataMultiplierMultiplier;
                Debug.Log("Activate with multiplier:" +multiplier + " "+SkillTransferData.data);
            }

            if (BonusAttributes != null&& BonusAttributes.Length>0)
            {
                if(attributesIsGrowths)
                    ApplyGrowths(level);
                else
                {
                    ApplyAttributes(level);
                }
            }

            if (BonusStats != null&& BonusStats.Length>0)
            {
                ApplyStats(level);
            }

            target.Stats.BonusMovement += BonusMov;
            if(level<cantoAmount.Length)
                target.GridComponent.Canto = cantoAmount[level];
        }

        private void ApplyStats(int level)
        {
            if (effectType == BonusEffectType.Effect)
            {
                if (level < BonusStats.Length)
                    target.Stats.BonusStatsFromEffects += BonusStats[level] * multiplier;
                else
                {
                    target.Stats.BonusStatsFromEffects += BonusStats[BonusStats.Length - 1] * multiplier;
                }
            }
            else if(effectType == BonusEffectType.Blessing)
            {
                if (level < BonusStats.Length)
                    target.Stats.BonusStatsFromBlessings += BonusStats[level] * multiplier;
                else
                {
                    target.Stats.BonusStatsFromBlessings += BonusStats[BonusStats.Length - 1]* multiplier;
                }
            }
            else if(effectType == BonusEffectType.Relic)
            {
                if (level < BonusStats.Length)
                    target.Stats.BonusStatsFromEquips += BonusStats[level] * multiplier;
                else
                {
                    target.Stats.BonusStatsFromEquips += BonusStats[BonusStats.Length - 1]* multiplier;
                }
            }
            
        }

        private void ApplyAttributes(int level)
        {
            if (effectType == BonusEffectType.Effect)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEffects +=
                        BonusAttributes[level]* multiplier;
                else
                {
                    target.Stats.BonusAttributesFromEffects +=
                        BonusAttributes[BonusAttributes.Length - 1]* multiplier;
                }
            }
            else if (effectType == BonusEffectType.Blessing)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromBlessings +=
                        BonusAttributes[level] * multiplier;
                else
                {
                    target.Stats.BonusAttributesFromBlessings +=
                        BonusAttributes[BonusAttributes.Length - 1]* multiplier;
                }
            }
            else if (effectType == BonusEffectType.Relic)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEquips +=
                        BonusAttributes[level] * multiplier;
                else
                {
                    target.Stats.BonusAttributesFromEquips +=
                        BonusAttributes[BonusAttributes.Length - 1]* multiplier;
                }
            }
            
        }
        private void ApplyGrowths(int level)
        {
            
            if (level < BonusAttributes.Length)
                target.Stats.BonusGrowths +=
                    BonusAttributes[level]* multiplier;
            else
            {
                target.Stats.BonusGrowths +=
                    BonusAttributes[BonusAttributes.Length - 1]* multiplier;
            }
            
        }

        void UnapplyAttributes(int level)
        {

            if (effectType == BonusEffectType.Effect)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEffects -= BonusAttributes[level] * multiplier;
                else
                {
                    target.Stats.BonusAttributesFromEffects -= BonusAttributes[^1] * multiplier;
                }
            }
            else if (effectType == BonusEffectType.Blessing)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromBlessings -= BonusAttributes[level] * multiplier;
                else
                {
                    target.Stats.BonusAttributesFromBlessings -= BonusAttributes[^1] * multiplier;
                }
            }
            else if (effectType == BonusEffectType.Relic)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEquips -= BonusAttributes[level] * multiplier;
                else
                {
                    target.Stats.BonusAttributesFromEquips -= BonusAttributes[^1] * multiplier;
                }
            }

        }
        void UnapplyGrowths(int level)
        {

            
            if (level < BonusAttributes.Length)
                target.Stats.BonusGrowths -= BonusAttributes[level] * multiplier;
            else
            {
                target.Stats.BonusGrowths -= BonusAttributes[^1] * multiplier;
            }
            
            

        }

        void UnapplyStats(int level)
        {
            if (effectType == BonusEffectType.Effect)
            {
                if (level < BonusStats.Length)
                    target.Stats.BonusStatsFromEffects -= BonusStats[level] * multiplier;
                else
                {
                    target.Stats.BonusStatsFromEffects -= BonusStats[^1] * multiplier;
                }
            }
            else if (effectType == BonusEffectType.Blessing)
            {
                if (level < BonusStats.Length)
                    target.Stats.BonusStatsFromBlessings -= BonusStats[level] * multiplier;
                else
                {
                    target.Stats.BonusStatsFromBlessings -= BonusStats[^1] * multiplier;
                }
            }
            else if (effectType == BonusEffectType.Relic)
            {
                if (level < BonusStats.Length)
                    target.Stats.BonusStatsFromEquips -= BonusStats[level] * multiplier;
                else
                {
                    target.Stats.BonusStatsFromEquips -= BonusStats[^1] * multiplier;
                }
            }

        }
        void RemoveBonuses(int level)
        {
            // if (skillTransferDataIsMultiplier && SkillTransferData != null&& SkillTransferData.data!=null)
            //     multiplier = (float)SkillTransferData.data * skillTransferDataMultiplierMultiplier;
            //USING LAST MULTIPLIER
            //"TODO remove actual added attributes because level can change");

            if (BonusAttributes != null && BonusAttributes.Length > 0)
            {
                if(attributesIsGrowths)
                    UnapplyGrowths(level);
                else
                    UnapplyAttributes(level);
                
            }

            if (BonusStats != null && BonusStats.Length > 0)
            {
                UnapplyStats(level);
            }
            target.Stats.BonusMovement -= BonusMov;
            
            if(cantoAmount.Length>0)
                target.GridComponent.Canto = 0;
            Debug.Log("BOOST STATS DEACTIVATED "+target.GridComponent.Canto);
        }

        

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            if (skillTransferDataIsMultiplier && SkillTransferData != null&& SkillTransferData.data!=null)
                multiplier = (float)SkillTransferData.data * skillTransferDataMultiplierMultiplier;

            if (level < BonusAttributes.Length)
            {
                string valueLabel = "";
                string attributeslabel = "";
                string upgLabel = "";
               
                if (attributesIsGrowths)
                {
                    attributeslabel = (BonusAttributes[level]*multiplier).GetTooltipText();
                    valueLabel =(BonusAttributes[level]*multiplier).GetTooltipGrowthValue();
                    // if(level<MAXLEVEL)
                    if (level < BonusAttributes.Length - 1)
                        level++;
                
                    upgLabel = (BonusAttributes[level]*multiplier).GetTooltipGrowthValue();
                    list.Add(new EffectDescription("Growths:", "", ""));
                }
                else
                {
                    attributeslabel = (BonusAttributes[level]*multiplier).GetTooltipText();
                    valueLabel =(BonusAttributes[level]*multiplier).GetTooltipValue();
                    // if(level<MAXLEVEL)
                    if (level < BonusAttributes.Length - 1)
                        level++;
                
                    upgLabel = (BonusAttributes[level]*multiplier).GetTooltipValue();
                }
                list.Add(new EffectDescription(attributeslabel, valueLabel, upgLabel));
            }
            if (level < BonusStats.Length)
            {
                List<string> bonusStatsLabels = (BonusStats[level]*multiplier).GetToolTipLabels();
                List<string> bonusStatsValues = (BonusStats[level]*multiplier).GetToolTipValues();

                // if(level<MAXLEVEL)
                if (level < BonusStats.Length - 1)
                    level++;

                List<string> bonusStatsUpgrades = (BonusStats[level]*multiplier).GetToolTipValues();

                for (int i = 0; i < bonusStatsLabels.Count; i++)
                {
                    list.Add(new EffectDescription(bonusStatsLabels[i], bonusStatsValues[i], bonusStatsUpgrades[i]));
                }
            }
            if(level<cantoAmount.Length)
                list.Add(new EffectDescription("Canto", ""+cantoAmount[level],  ""+cantoAmount[level+1]));


            return list;
        }
    }

}