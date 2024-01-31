using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/ItemModifier", fileName = "MetaUpgrade1")]
public class ItemModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<ResourceType, int> startResource;
    public SerializableDictionary<ItemBP, int> startItems;
    public SerializableDictionary<ItemModifierType, int> itemModifiers;
    public override void Activate(int level)
    {
        foreach (KeyValuePair<ResourceType, int> valuePair in startResource)
        {
            switch (valuePair.Key)
            {
                case ResourceType.Gold:
                    Party.StartGold = valuePair.Value; break;
            }
        }
        foreach (KeyValuePair<ItemBP, int> valuePair in startItems)
        {
            Convoy.StartItems.Add(valuePair.Key, valuePair.Value);
        }

        foreach (KeyValuePair<ItemModifierType, int> valuePair in itemModifiers)
        {
            switch (valuePair.Key)
            {
                case ItemModifierType.HealthPotionsHeal:
                    HealthPotion.ExtraHealAmount = valuePair.Value;break;
            }
        }
    }
}
public enum ItemModifierType
{
    HealthPotionsHeal
}