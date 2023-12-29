using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Modifier", fileName = "MetaUpgrade1")]
public class ModifierMetaUpgradeMixin: MetaUpgradeMixin
{
    public float[] percentage;
    public ModifierType ModifierType;
}


public enum ModifierType
{
    Experience,
    Grace,
    Gold,
    BondExp,
    CurseResistance,
    RelicDropRate,
    GemstoneDropRate,
    RareEncounterRate,
    FoodHealRate,
    RestHealRate,
    FlameLevelRate,
    KillExpRate,
    AssistExpRate,
    HealingRate,
    EliteBattlesRate
}