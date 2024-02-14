using System.Collections.Generic;
using Game.GUI.EncounterUI.Inn;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/GoldModifier", fileName = "MetaUpgrade1")]
public class GoldModifierMetaUpgradeMixin: MetaUpgradeMixin
{
    public float[] percentage;
    public GoldModifierType ModifierType;
    public override void Activate(int level)
    {
        switch (ModifierType)
        {
            case GoldModifierType.PriceBuying:
                Merchant.PriceRateBuying = percentage[level]; break;
            case GoldModifierType.PriceSelling:
                Merchant.PriceRateSelling = percentage[level]; break;
            case GoldModifierType.PriceChurch:
                Church.PriceRate = percentage[level];
                break;
            case GoldModifierType.PriceInn:
                UIInnController.PriceRate = percentage[level];
                break;
            case GoldModifierType.PriceSmithy:
                Smithy.PriceRate = percentage[level];
                break;
        }
    }
    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        int upgLevel = level;
        if (upgLevel < percentage.Length-1)
            upgLevel++;
        list.Add(new EffectDescription(""+ModifierType.EnumToString()+":", TextUtility.FormatPercentage(percentage[level]), TextUtility.FormatPercentage(percentage[upgLevel])));
       
        return list;
    
    }
}
public enum GoldModifierType
{
    PriceChurch,
    PriceSmithy,
    PriceSelling,
    PriceBuying,
    PriceInn,
}