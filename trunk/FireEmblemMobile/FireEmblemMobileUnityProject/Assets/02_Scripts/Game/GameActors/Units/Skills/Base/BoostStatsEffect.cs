using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;

using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/Stats", fileName = "StatsEffect")]
    public class BoostStatsEffect : SelfTargetSkillEffectMixin
    {
      
        public Attributes[] BonusAttributes;
        public CombatStats[] BonusStats;
        public int BonusMov; 
        [SerializeField] private int[] cantoAmount;
        public float multiplier = 1;
        public bool skillTransferDataIsMultiplier;
        public float skillTransferDataMultiplierMultiplier;
     
        public SkillTransferData SkillTransferData;
        private bool activated = false;
        public override void Activate(Unit target, int level)
        {

            if (skillTransferDataIsMultiplier && SkillTransferData != null&& SkillTransferData.data!=null)
            {
                Debug.Log("GUID:" +SkillTransferData.guid);
                multiplier = (float)SkillTransferData.data * skillTransferDataMultiplierMultiplier;
            }

           

            if (BonusAttributes != null&& BonusAttributes.Length>0)
            {
                if(level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEffects += BonusAttributes[level]*multiplier;
                else
                {
                    target.Stats.BonusAttributesFromEffects += BonusAttributes[BonusAttributes.Length-1]*multiplier;
                }
            }

            if (BonusStats != null&& BonusStats.Length>0)
            {
                if(level < BonusStats.Length)
                    target.Stats.BonusStatsFromEffects += BonusStats[level]*multiplier;
                else
                {
                    target.Stats.BonusStatsFromEffects += BonusStats[BonusStats.Length-1]*multiplier;
                }
            }
            if(level<cantoAmount.Length)
                target.GridComponent.Canto = cantoAmount[level];
            activated = true;
        }

        public override void Deactivate(Unit target, int level)
        {
            if (!activated)
                return;
            activated = false;
            if (skillTransferDataIsMultiplier && SkillTransferData != null&& SkillTransferData.data!=null)
                multiplier = (float)SkillTransferData.data * skillTransferDataMultiplierMultiplier;

           //"TODO remove actual added attributes because level can change");
            if (BonusAttributes != null && BonusAttributes.Length > 0)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEffects -= BonusAttributes[level]*multiplier;
                else
                {
                    target.Stats.BonusAttributesFromEffects -= BonusAttributes[BonusAttributes.Length - 1]*multiplier;
                }
            }

            if (BonusStats != null&& BonusStats.Length>0)
            {
                if(level < BonusStats.Length)
                    target.Stats.BonusStatsFromEffects -= BonusStats[level]*multiplier;
                else
                {
                    target.Stats.BonusStatsFromEffects -= BonusStats[BonusStats.Length-1]*multiplier;
                }
            }
            if(cantoAmount.Length>0)
                target.GridComponent.Canto = 0;
            Debug.Log("BOOST STATS DEACTIVATED "+target.GridComponent.Canto);
        }

        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            if (level < BonusAttributes.Length)
            {
                string attributeslabel = BonusAttributes[level].GetTooltipText();
                string valueLabel =BonusAttributes[level].GetTooltipValue();
                // if(level<MAXLEVEL)
                if (level < BonusAttributes.Length - 1)
                    level++;
                
                string upgLabel = BonusAttributes[level].GetTooltipValue();
                list.Add(new EffectDescription(attributeslabel, "+"+valueLabel, "+"+upgLabel));
            }
            if (level < BonusStats.Length)
            {
                List<string> bonusStatsLabels = BonusStats[level].GetToolTipLabels();
                List<string> bonusStatsValues = BonusStats[level].GetToolTipValues();

                // if(level<MAXLEVEL)
                if (level < BonusStats.Length - 1)
                    level++;

                List<string> bonusStatsUpgrades = BonusStats[level].GetToolTipValues();

                for (int i = 0; i < bonusStatsLabels.Count; i++)
                {
                    list.Add(new EffectDescription(bonusStatsLabels[i], "+"+bonusStatsValues[i], "+"+bonusStatsUpgrades[i]));
                }
            }
            if(level<cantoAmount.Length)
                list.Add(new EffectDescription("Canto", ""+cantoAmount[level],  ""+cantoAmount[level+1]));


            return list;
        }
    }

}