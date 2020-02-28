using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    [CreateAssetMenu(menuName ="GameData/Inventory", fileName ="Inventory")]
    public class Inventory :ScriptableObject
    {
        public const int MAX_ITEMS = 6;
        [HideInInspector]
        public Human owner;
        public List<Item> items;


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
