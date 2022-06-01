using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items.Weapons;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.Humans
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Human", fileName = "Human")]
    public class Human : Unit
    {
        public static event Action OnEquippedWeapon;
        public RpgClass Class;

        public Weapon EquippedWeapon;

       // public Inventory Inventory;

       public Relic EquippedRelic;


       

        public Human()
        {
           
        }

        public override void Initialize()
        {
            base.Initialize();
            Stats.AttackRanges.Clear();
            if (EquippedWeapon != null)
            {
                foreach (int r in EquippedWeapon.AttackRanges)
                    Stats.AttackRanges.Add(r);
            }
        }

        // public void OnEnable()
        // {
        //     // if (Inventory == null)
        //     //     Inventory = Instantiate(CreateInstance<Inventory>());
        //     // Inventory.Owner = this;
        //     base.OnEnable();
        //     
        //     Equip(EquippedWeapon);
        // }


        protected override void HandleCloned(Unit clone)
        {
            base.HandleCloned(clone);
            var human = (Human) clone;
            human.EquippedWeapon = (Weapon)EquippedWeapon?.Clone();
            //human.Inventory = (Inventory)Inventory.Clone();
            human.Class = Class;
            
        }
        public Dictionary<WeaponType,string> WeaponProficiencies()
        {
            //PERFORMANCE PROBLEM!!!
            //var dict = new Dictionary<WeaponType, string>
            //{
            //    { WeaponType.Sword, GetWeaponProficiency(WeaponType.Sword) },
            //    { WeaponType.Spear, GetWeaponProficiency(WeaponType.Spear) },
            //    { WeaponType.Axe, GetWeaponProficiency(WeaponType.Axe) },
            //    { WeaponType.Bow, GetWeaponProficiency(WeaponType.Bow) },
            //    { WeaponType.Magic, GetWeaponProficiency(WeaponType.Magic) },
            //    { WeaponType.Staff, GetWeaponProficiency(WeaponType.Staff) }
            //};

            //return dict;
            return null;
        }

        public bool CanUseWeapon(Weapon w)
        {
            return true;
        }
        public void Equip(EquipableItem e)
        {

            switch (e.EquipmentSlotType)
            {
                //case EquipmentSlotType.Armor: Debug.LogError("TODO Equip Armor!"); break;
                case EquipmentSlotType.Weapon: 
                    Equip((Weapon) e);break;
                case EquipmentSlotType.Relic: Debug.LogError("TODO Equip Relic!"); break;
            }
        }
        public void Equip(Weapon w)
        {
            
            Stats.AttackRanges.Clear();
            EquippedWeapon = w;
            foreach (int r in w.AttackRanges) Stats.AttackRanges.Add(r);
            Debug.Log("Equip " + w.name + " on " + name + " " + w.AttackRanges.Length+" "+ Stats.AttackRanges.Count);
            OnEquippedWeapon?.Invoke();
        }
        public void AutoEquip()
        {
           
            // if (EquippedWeapon == null)
            // {
            //     Equip((Weapon)Inventory.Items.First(a=> a is Weapon weapon && CanUseWeapon(weapon)));
            // }
        }

        public bool CanEquip(EquipableItem eitem)
        {
            Debug.Log("TODO Check if item is equipable");
            return true;
        }

        public void InitEquipment()
        {
            if(EquippedWeapon!=null)
                EquippedWeapon=Instantiate(EquippedWeapon);
            if(EquippedRelic!=null)
                EquippedRelic=Instantiate(EquippedRelic);
        }
    }

    
}