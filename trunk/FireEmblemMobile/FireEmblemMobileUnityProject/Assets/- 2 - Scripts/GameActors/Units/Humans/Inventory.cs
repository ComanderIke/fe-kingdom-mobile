using System.Collections.Generic;
using Assets.GameActors.Items;
using Assets.GameActors.Items.Weapons;
using UnityEngine;

namespace Assets.GameActors.Units.Humans
{
    [CreateAssetMenu(menuName = "GameData/Inventory", fileName = "Inventory")]
    public class Inventory : ScriptableObject
    {
        public const int MAX_ITEMS = 6;
        public List<Item> Items;

        [HideInInspector] public Human Owner;

        public void AddItem(Item i)
        {
            Items.Add(i);
            if (i is Weapon weapon)
                if (Owner.EquippedWeapon == null)
                    weapon.Equip(Owner);
        }

        public void DropItem(Item i)
        {
            Items.Remove(i);
        }

        public void UseItem(Item i)
        {
            i.Use(Owner);
            if (i.NumberOfUses <= 0) Items.Remove(i);
        }
    }
}