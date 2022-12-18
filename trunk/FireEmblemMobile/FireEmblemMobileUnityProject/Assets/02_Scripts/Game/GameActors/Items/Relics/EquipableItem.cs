using System;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public abstract class EquipableItem : Item
    {
        public EquipmentSlotType EquipmentSlotType;
        public EquipableItem(string name, string description, int cost,int rarity,int maxStack, Sprite sprite, EquipmentSlotType slotType) : base(name, description, cost,rarity, maxStack,sprite)
        {
            EquipmentSlotType = slotType;
        }
    }
}