using System.Collections.Generic;
using Game.GameActors.Players;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Modifier", fileName = "MetaUpgrade1")]
public class ModifierMetaUpgradeMixin: MetaUpgradeMixin
{
    public float[] percentage;
    public ModifierType ModifierType;

    public override void Activate(int level)
    {
        switch (ModifierType)
        {
            case ModifierType.Growth:
                Player.Instance.Modifiers.GrowthIncrease = percentage[level];
                break;
            case ModifierType.Experience:
                Player.Instance.Modifiers.ExperienceGain = percentage[level];
                break;
            case ModifierType.Gold: 
                Player.Instance.Modifiers.GoldGain = percentage[level];
                break;
            case ModifierType.Grace: 
                Player.Instance.Modifiers.GraceGain = percentage[level];
                break;
            case ModifierType.HealingRate: 
                Player.Instance.Modifiers.HealingRate = percentage[level];
                break;
            case ModifierType.AssistExpRate: 
                Player.Instance.Modifiers.AssistExpRate = percentage[level];
                break;
            case ModifierType.BondExp: 
                Player.Instance.Modifiers.BondExpGain = percentage[level];
                break;
            case ModifierType.CurseResistance: 
                Player.Instance.Modifiers.CurseResistance = percentage[level];
                break;
            case ModifierType.RelicDropRate: 
                Player.Instance.Modifiers.RelicDropRate = percentage[level];
                break;
            case ModifierType.EliteBattlesRate: 
                Player.Instance.Modifiers.EliteBattleRate = percentage[level];
                break;
            case ModifierType.FlameLevelRate: 
                Player.Instance.Modifiers.FlameLevelRate = percentage[level];
                break;
            case ModifierType.FoodHealRate: 
                Player.Instance.Modifiers.FoodHealRate = percentage[level];
                break;
            case ModifierType.GemstoneDropRate: 
                Player.Instance.Modifiers.GemstoneDropRate = percentage[level];
                break;
            case ModifierType.KillExpRate: 
                Player.Instance.Modifiers.KillExpRate = percentage[level];
                break;
            case ModifierType.RestHealRate: 
                Player.Instance.Modifiers.RestHealRate = percentage[level];
                break;
            case ModifierType.RareEncounterRate: 
                Player.Instance.Modifiers.RareEncounterRate = percentage[level];
                break;
            case ModifierType.RareMerchants: 
                Player.Instance.Modifiers.RareMerchants = percentage[level];
                break;
            case ModifierType.RareSkillRarity:
                Player.Instance.Modifiers.RareSkillRarity = percentage[level];
                break;
            case ModifierType.EpicSkillRarity:
                Player.Instance.Modifiers.EpicSkillRarity = percentage[level];
                break;
            case ModifierType.LegendarySkillRarity:
                Player.Instance.Modifiers.LegendarySkillRarity = percentage[level];
                break;
            case ModifierType.SkillActivation:
                Player.Instance.Modifiers.SkillActivation = percentage[level];
                break;
            case ModifierType.BonusHeal:
                Player.Instance.Modifiers.BonusHeal = (int)percentage[level];
                break;
            case ModifierType.GemStoneEffect:
                Player.Instance.Modifiers.GemStoneEffect = percentage[level];
                break;
            case ModifierType.ThanatosAppearRate:
                Player.Instance.Modifiers.ThanatosAppearRate = percentage[level];
                break;
          
            
        }
    }
    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        if (level >= percentage.Length)
            return list;
        int upgLevel = level;
        if (upgLevel < percentage.Length-1)
            upgLevel++;
        list.Add(new EffectDescription(ModifierType.EnumToString()+":", TextUtility.FormatPercentage(percentage[level]),TextUtility.FormatPercentage(percentage[upgLevel])));

       
        return list;
    }
}


public enum ModifierType
{
    Experience,
    Grace,
    Gold,
    BondExp,
    Growth,
    CurseResistance,
    RelicDropRate,
    GemstoneDropRate,
    RareEncounterRate,
    RareMerchants,
    FoodHealRate,
    RestHealRate,
    FlameLevelRate,
    KillExpRate,
    AssistExpRate,
    HealingRate,
    EliteBattlesRate,
    RareSkillRarity,
    EpicSkillRarity,
    LegendarySkillRarity,
    SkillActivation,
    BonusHeal,
    GemStoneEffect,
    ThanatosAppearRate,
    
}