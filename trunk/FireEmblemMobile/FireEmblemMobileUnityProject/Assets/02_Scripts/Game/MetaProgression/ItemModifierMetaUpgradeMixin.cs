using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/ItemModifier", fileName = "MetaUpgrade1")]
public class ItemModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<ResourceType, int> startResource;
    public SerializableDictionary<ItemBP, int> startItems;
    public SerializableDictionary<ItemModifierType, int> itemModifiers;
}
public enum ItemModifierType
{
    HealthPotionsHeal,
    HolyWaterEffect,
}