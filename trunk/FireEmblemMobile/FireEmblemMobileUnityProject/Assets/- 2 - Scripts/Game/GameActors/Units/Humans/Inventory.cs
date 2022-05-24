using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.GameActors.Units.Humans
{
    [CreateAssetMenu(menuName = "GameData/Inventory", fileName = "Inventory")]
    public class Inventory : ScriptableObject, ICloneable
    {
        public const int MAX_ITEMS = 6;
        public List<Item> Items;

        [HideInInspector] public Human Owner;

        public void AddItem(Item i)
        {
            Items.Add(i);
            if (i is Weapon weapon)
            {
                Owner?.AutoEquip();
            }
        }

        public void DropItem(Item i)
        {
            Items.Remove(i);
        }

        public void UseItem(ConsumableItem i, Convoy convoy)
        {
            i.Use(Owner, convoy);
            if (i.NumberOfUses <= 0) Items.Remove(i);
        }

        public object Clone()
        {
            var clone = (Inventory)MemberwiseClone();
            clone.Items = new List<Item>();
            foreach (Item i in Items)
            {
                clone.Items.Add((Item)i.Clone());
            }
            return clone;
        }
    }
}