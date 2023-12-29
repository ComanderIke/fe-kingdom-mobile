using Game.GUI.EncounterUI.Inn;
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
}
public enum GoldModifierType
{
    PriceChurch,
    PriceSmithy,
    PriceSelling,
    PriceBuying,
    PriceInn,
}