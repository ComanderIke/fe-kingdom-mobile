using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [System.Serializable]
    public class Weapon : Item
    {
        public int Dmg { get; set; }
        public int Price { get; set; }
        public int Hit { get; set; }
        public int Range { get; set; }
        public int Crit { get; set; }
        public WeaponCategory WeaponType { get; set; }

        public Weapon(string name, string description, WeaponCategory type, int dmg, int hit, int crit, int price, int range, Sprite sprite):base(name, description, 1, sprite)
        {
            WeaponType = type;
            Dmg = dmg;
            Hit = hit;
            Crit = crit;
            Price = price;
            Range = range;
        }

        public override void use(Human character)
        { 
            //Equip Weapon
        }

    }
}
