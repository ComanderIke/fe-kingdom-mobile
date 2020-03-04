using System;
using System.Collections.Generic;
using Assets.GameActors.Items.Weapons;
using UnityEngine;

namespace Assets.GameActors.Units.Humans
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Human", fileName = "Human")]
    public class Human : Unit
    {
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
            else
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
        public Dictionary<WeaponType,string> WeaponProficiencies()
        {
            var dict = new Dictionary<WeaponType, string>
            {
                { WeaponType.Sword, GetWeaponProficiency(WeaponType.Sword) },
                { WeaponType.Spear, GetWeaponProficiency(WeaponType.Spear) },
                { WeaponType.Axe, GetWeaponProficiency(WeaponType.Axe) },
                { WeaponType.Bow, GetWeaponProficiency(WeaponType.Bow) },
                { WeaponType.Magic, GetWeaponProficiency(WeaponType.Magic) },
                { WeaponType.Staff, GetWeaponProficiency(WeaponType.Staff) }
            };

            return dict;
        }
    }
}