using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/EncounterNodeModifier", fileName = "MetaUpgrade1")]
public class EncounterNodeModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<EncounterNodeModifierType, int> modifiers;
}

public enum EncounterNodeModifierType
{
    MerchantSlots,
    SmithingMaxUpgrade,
    FoodSlotsInn
}