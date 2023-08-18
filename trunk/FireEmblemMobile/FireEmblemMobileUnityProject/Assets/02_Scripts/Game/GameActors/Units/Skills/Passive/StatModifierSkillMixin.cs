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
        public CombatStats[] BonusStats;
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

            foreach (var skilleffect in skillEffectMixins)
            {
                if (skilleffect is SelfTargetSkillEffectMixin stsm)
                {
                    stsm.Activate(unit, skill.level);
                }
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
                foreach (var skilleffect in skillEffectMixins)
                {
                    if (skilleffect is SelfTargetSkillEffectMixin stsm)
                    {
                        stsm.Deactivate(unit, skill.level);
                    }
                }
            }

            bound = false;
            base.UnbindFromUnit(unit, skill);
        }
          public override List<EffectDescription> GetEffectDescription(Unit unit, int level)
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
                list.Add(new EffectDescription(attributeslabel, valueLabel, upgLabel));
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
                    list.Add(new EffectDescription(bonusStatsLabels[i], bonusStatsValues[i], bonusStatsUpgrades[i]));
                }
            }

            return list;
        }
          
    
        
    }
}