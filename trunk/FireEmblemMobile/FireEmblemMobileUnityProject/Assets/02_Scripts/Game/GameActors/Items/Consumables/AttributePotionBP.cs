using System;
using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Consumables/AttributePotion", fileName = "AttributePotion")]
    public class AttributePotionBP : ConsumableItemBp
    {
        public int value;
        public AttributeType attributeType;
        public override Item Create()
        {
            return new AttributePotion(name, description, cost, rarity,maxStack,sprite, target, value, attributeType);

        }
    }
}