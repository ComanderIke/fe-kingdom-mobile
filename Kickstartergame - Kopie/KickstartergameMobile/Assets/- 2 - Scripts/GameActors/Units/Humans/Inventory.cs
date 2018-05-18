﻿using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    public class Inventory
    {
        public const int MAX_ITEMS = 6;

        private Human owner;
        public List<Item> items;

        public Inventory(Human owner)
        {
            this.owner = owner;
            items = new List<Item>();
            items.Capacity = MAX_ITEMS;
        }

        public void AddItem(Item i)
        {
            items.Add(i);
            if(i is Weapon)
            {
                if(owner.EquipedWeapon==null)
                    ((Weapon)i).Equip(owner);
            }
        }

        public void DropItem(Item i)
        {
            items.Remove(i);
        }

        public void UseItem(Item i)
        {
            i.Use(owner);
            if (i.NumberOfUses <= 0)
            {
                items.Remove(i);
            }
        }
    }
}
