using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items.Weapons;
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

        public Inventory Inventory;
        public SkillManager SkillManager { get; set; }
        public SpecialAttackManager SpecialAttackManager { get; set; }

        [SerializeField] private string[] weaponProficiencyLevels;

        public Human()
        {
            weaponProficiencyLevels = new string[]{
                "E","E","E","E","E","E"
            };
        }

        public new void OnEnable()
        {
            base.OnEnable();
            if (Inventory == null)
                Inventory = Instantiate(CreateInstance<Inventory>());
            Inventory.Owner = this;
            SkillManager = new SkillManager();
            SpecialAttackManager = new SpecialAttackManager();
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
            human.Inventory = (Inventory)Inventory.Clone();
            human.Class = Class;
            human.SkillManager = (SkillManager) SkillManager.Clone();
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
            //Debug.Log("Equip " + w.name + " on " + Name + " " + w.AttackRanges.Length+" "+ Stats.AttackRanges.Count);
            OnEquippedWeapon?.Invoke();
        }
        public void AutoEquip()
        {
           
            if (EquippedWeapon == null)
            {
                Equip((Weapon)Inventory.Items.First(a=> a is Weapon weapon && CanUseWeapon(weapon)));
            }
        }
    }
}