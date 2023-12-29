using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/GoldModifier", fileName = "MetaUpgrade1")]
public class GoldModifierMetaUpgradeMixin: MetaUpgradeMixin
{
    public float[] percentage;
    public GoldModifierType ModifierType;
    public override void Activate(int level)
    {
        throw new System.NotImplementedException();
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