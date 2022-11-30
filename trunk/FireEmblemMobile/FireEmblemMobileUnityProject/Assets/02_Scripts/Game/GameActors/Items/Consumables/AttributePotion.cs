using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    public class AttributePotion : ConsumableItem
    {
        public int value;
        public AttributeType attributeType;
        public AttributePotion(string name, string description, int cost,int rarity, Sprite sprite, ItemTarget target, int value, AttributeType type) : base(name, description, cost, rarity,sprite, target)
        {
            this.value = value;
            this.attributeType = type;
        }
    }
}