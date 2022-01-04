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

       public Armor EquippedArmor;
       public Relic EquippedRelic;
       public Consumable EquippedConsumable;

        [SerializeField] private string[] weaponProficiencyLevels;

        public Human()
        {
            weaponProficiencyLevels = new string[]{
                "E","E","E","E","E","E"
            };
        }

        public void OnEnable()
        {
            // if (Inventory == null)
            //     Inventory = Instantiate(CreateInstance<Inventory>());
            // Inventory.Owner = this;
            base.OnEnable();
            
            Equip(EquippedWeapon);
        }
      
        public string GetWeaponProficiency(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Sword: return weaponProficiencyLevels[0];
                case WeaponType.Spear: return weaponProficiencyLevels[1];
                case WeaponType.Axe: return weaponProficiencyLevels[2];
                case WeaponType.Bow: return weaponProficiencyLevels[3];
                case WeaponType.Magic: return weaponProficiencyLevels[4];
                case WeaponType.Staff: return weaponProficiencyLevels[5];
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
        }

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
    }

    
}