using Game.GameActors.Units;
using Game.GameActors.Units.Numbers;
using Game.WorldMapStuff.Model;
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

        public override void Use(Unit character, Convoy convoy)
        {
            Debug.Log("use AttributePotion "+value+" "+attributeType);
            character.Stats.BaseAttributes.IncreaseAttribute(value, attributeType);
            base.Use(character, convoy);
        }
    }
}