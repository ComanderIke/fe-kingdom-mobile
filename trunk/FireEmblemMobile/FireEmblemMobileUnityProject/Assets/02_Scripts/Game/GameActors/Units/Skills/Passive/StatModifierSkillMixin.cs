using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public enum EffectOrigin
    {
        Equipment,
        Effects
    }
   [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/Passive/StatModifier", fileName = "StatModifierMixin")]
    public class StatModifierSkillMixin : PassiveSkillMixin
    {
        

        public EffectOrigin EffectOrigin = EffectOrigin.Effects;
        public Attributes[] BonusAttributes;
        public BonusStats[] BonusStats;
        private bool bound = false;
        public override void BindToUnit(Unit unit, Skill skill)
        {
            bound = true;
            base.BindToUnit(unit, skill);
            Debug.Log("Bind Skill "+skill.Name+" "+unit.Name+" "+skill.Level);
            switch (EffectOrigin)
            {
                case EffectOrigin.Effects: 
                    if(BonusAttributes!=null&& BonusAttributes.Length > skill.Level)
                        unit.Stats.BonusAttributesFromEffects += BonusAttributes[skill.Level];
                    if(BonusStats!=null&& BonusStats.Length > skill.Level)
                        unit.Stats.BonusStatsFromEffects += BonusStats[skill.Level];
                    break;
                case EffectOrigin.Equipment: 
                    if(BonusAttributes!=null&& BonusAttributes.Length > skill.Level)
                        unit.Stats.BonusAttributesFromEquips += BonusAttributes[skill.Level];
                    if(BonusStats!=null&& BonusStats.Length > skill.Level)
                        unit.Stats.BonusStatsFromEquips += BonusStats[skill.Level];
                    break;
            }
           
            
        }
        
        public override void UnbindFromUnit(Unit unit, Skill skill)
        {

            
            if (bound)
            {
                Debug.Log("Unbind Skill " + skill.Name + " " + unit.Name + " " + skill.Level);
                switch (EffectOrigin)
                {
                    case EffectOrigin.Effects:
                        if (BonusAttributes != null && BonusAttributes.Length > skill.Level)
                            unit.Stats.BonusAttributesFromEffects -= BonusAttributes[skill.Level];
                        if (BonusStats != null && BonusStats.Length > skill.Level)
                            unit.Stats.BonusStatsFromEffects -= BonusStats[skill.Level];
                        break;
                    case EffectOrigin.Equipment:
                        if (BonusAttributes != null && BonusAttributes.Length > skill.Level)
                            unit.Stats.BonusAttributesFromEquips -= BonusAttributes[skill.Level];
                        if (BonusStats != null && BonusStats.Length > skill.Level)
                            unit.Stats.BonusStatsFromEquips -= BonusStats[skill.Level];
                        break;
                }
            }

            bound = false;
            base.UnbindFromUnit(unit, skill);
        }
          public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
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
            if(level<MAXLEVEL)
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