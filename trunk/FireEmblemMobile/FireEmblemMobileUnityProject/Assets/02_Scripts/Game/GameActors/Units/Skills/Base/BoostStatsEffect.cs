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
        public BonusStats[] BonusStats;
        public override void Activate(Unit target, int level)
        {
           
            if (BonusAttributes != null&& BonusAttributes.Length>0)
            {
                if(level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEffects += BonusAttributes[level];
                else
                {
                    target.Stats.BonusAttributesFromEffects += BonusAttributes[BonusAttributes.Length-1];
                }
            }

            if (BonusStats != null&& BonusStats.Length>0)
            {
                if(level < BonusStats.Length)
                    target.Stats.BonusStatsFromEffects += BonusStats[level];
                else
                {
                    target.Stats.BonusStatsFromEffects += BonusStats[BonusStats.Length-1];
                }
            }
               
        }

        public override void Deactivate(Unit target, int level)
        {
            Debug.Log("TODO remove actual added attributes because level can change");
            if (BonusAttributes != null && BonusAttributes.Length > 0)
            {
                if (level < BonusAttributes.Length)
                    target.Stats.BonusAttributesFromEffects -= BonusAttributes[level];
                else
                {
                    target.Stats.BonusAttributesFromEffects -= BonusAttributes[BonusAttributes.Length - 1];
                }
            }

            if (BonusStats != null&& BonusStats.Length>0)
            {
                if(level < BonusStats.Length)
                    target.Stats.BonusStatsFromEffects -= BonusStats[level];
                else
                {
                    target.Stats.BonusStatsFromEffects -= BonusStats[BonusStats.Length-1];
                }
            }
        }

        public override List<EffectDescription> GetEffectDescription(int level)
        {
            var list = new List<EffectDescription>();
              string attributeslabel = ""+(BonusAttributes[level].STR!=0?Attributes.GetAsText(0)+"/":"") //  either grant STR/SPD/SKL   5/4/3 -> 5/5/
            +(BonusAttributes[level].DEX!=0?Attributes.GetAsText(1)+"/":"")
            +(BonusAttributes[level].INT!=0?Attributes.GetAsText(2)+"/":"")
            +(BonusAttributes[level].AGI!=0?Attributes.GetAsText(3)+"/":"")
            +(BonusAttributes[level].CON!=0?Attributes.GetAsText(4)+"/":"")
            +(BonusAttributes[level].LCK!=0?Attributes.GetAsText(5)+"/":"")
            +(BonusAttributes[level].DEF!=0?Attributes.GetAsText(6)+"/":"")
            +(BonusAttributes[level].FAITH!=0?Attributes.GetAsText(7)+"/":"")
                ;
            string valueLabel = ""+(BonusAttributes[level].STR!=0?BonusAttributes[level].STR+"/":"") //  either grant STR/SPD/SKL   5/4/3 -> 5/5/
                                +(BonusAttributes[level].DEX!=0?BonusAttributes[level].DEX+"/":"")
                                +(BonusAttributes[level].INT!=0?BonusAttributes[level].INT+"/":"")
                                +(BonusAttributes[level].AGI!=0?BonusAttributes[level].AGI+"/":"")
                                +(BonusAttributes[level].CON!=0?BonusAttributes[level].CON+"/":"")
                                +(BonusAttributes[level].LCK!=0?BonusAttributes[level].LCK+"/":"")
                                +(BonusAttributes[level].DEF!=0?BonusAttributes[level].DEF+"/":"")
                                +(BonusAttributes[level].FAITH!=0?BonusAttributes[level].FAITH+"/":"");
            if(valueLabel.Length>0)
                valueLabel.Remove(valueLabel.Length-2, 1);
           // if(level<MAXLEVEL)
           if(level< BonusAttributes.Length-1)
                level++;
                
          
            string upgLabel = ""+(BonusAttributes[level].STR!=0?BonusAttributes[level].STR+"/":"")
                                +(BonusAttributes[level].DEX!=0?BonusAttributes[level].DEX+"/":"")
                                +(BonusAttributes[level].INT!=0?BonusAttributes[level].INT+"/":"")
                                +(BonusAttributes[level].AGI!=0?BonusAttributes[level].AGI+"/":"")
                                +(BonusAttributes[level].CON!=0?BonusAttributes[level].CON+"/":"")
                                +(BonusAttributes[level].LCK!=0?BonusAttributes[level].LCK+"/":"")
                                +(BonusAttributes[level].DEF!=0?BonusAttributes[level].DEF+"/":"")
                                +(BonusAttributes[level].FAITH!=0?BonusAttributes[level].FAITH+"/":"");
            if(upgLabel.Length>0)
                upgLabel.Remove(valueLabel.Length-2, 1);
            
            list.Add(new EffectDescription(attributeslabel, valueLabel, upgLabel));
            return list;
        }
    }

}