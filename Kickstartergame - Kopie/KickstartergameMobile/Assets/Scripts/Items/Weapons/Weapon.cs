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
        public int dmg;
        public int price;
        public int hit;
        public int range;
        public int crit;
		public Sprite attack;
		public Sprite attack2;
        public WeaponCategory weaponType;

        public Weapon(string name, string description, WeaponCategory type, int dmg, int hit, int crit, int price, int range, GameObject go, Sprite sprite):base(name, description, 1, sprite, go)

        {
            this.weaponType = type;
            this.dmg = dmg;
            this.hit = hit;
            this.crit = crit;
            this.price = price;
            this.range = range;
			this.attack = attack;
			this.attack2 = attack2;
        }

        public override void use(global::Character character)
        { 
			// USE WEAPON EVENT
            //GameObject.Find("ItemMessage").GetComponent<ItemMessage>().Show(this);
            //character.items.Remove(this);
            //character.items.Add(character.EquipedWeapon);
            //character.EquipedWeapon = this;
            
            
        }

    }
}
