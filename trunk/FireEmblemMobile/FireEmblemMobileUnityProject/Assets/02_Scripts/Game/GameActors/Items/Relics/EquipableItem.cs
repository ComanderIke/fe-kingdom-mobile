using System;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public abstract class EquipableItem : Item
    {
   
        public EquipableItem(string name, string description, int cost,int rarity,int maxStack, Sprite sprite) : base(name, description, cost,rarity, maxStack,sprite)
        {
           
        }
    }
}