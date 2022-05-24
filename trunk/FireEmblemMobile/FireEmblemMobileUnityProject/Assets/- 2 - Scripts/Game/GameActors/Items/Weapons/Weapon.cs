using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class WeaponAttributes
    {
        public int Dmg=3;
        public int Hit=70;
        public int Crit=0;
        public int Weight=1;
        public List<WeaponMixin> WeaponMixins;
        public int upgradeGoldCost = 50;
        public int upgradeSmithingStoneCost = 1;
    }
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Weapons/Weapon", fileName = "Weapon")]
    public class Weapon : EquipableItem
    {
        
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] public WeaponAttributes[] WeaponAttributes;
        public int weaponLevel = 1;
        public int maxLevel = 5;
        
        public WeaponType WeaponType;
        public DamageType DamageType;

        public void OnEnable()
        {
            EquipmentSlotType = EquipmentSlotType.Weapon;
        }


        public int GetDamage()
        {
            return WeaponAttributes[weaponLevel].Dmg;
        }
        public int GetHit()
        {
            return WeaponAttributes[weaponLevel].Hit;
        }
        public int GetCrit()
        {
            return WeaponAttributes[weaponLevel].Crit;
        }
        public int GetWeight()
        {
            return WeaponAttributes[weaponLevel].Weight;
        }
        public int GetUpgradeCost()
        {
            return WeaponAttributes[weaponLevel].upgradeGoldCost;
        }
        public int GetUpgradeSmithingStoneCost()
        {
            return WeaponAttributes[weaponLevel].upgradeSmithingStoneCost;
        }
      

      

        public void OnAttack(Unit attacker, Unit defender)
        {
            foreach (var mixin in WeaponAttributes[weaponLevel].WeaponMixins) mixin.OnAttack(attacker, defender);
        }

        public void Upgrade()
        {
            weaponLevel++;
            if (weaponLevel > maxLevel)
                weaponLevel = maxLevel;
        }

        public object GetUpgradeableWeight()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel+1].Weight;
            else
                return 0;
        }

        public object GetUpgradeableCrit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel+1].Crit;
            else
                return 0;
        }

        public object GetUpgradeableHit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel+1].Hit;
            else
                return 0;
        }

        public object GetUpgradeableDmg()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel + 1].Dmg;
            else
                return 0;
        }
    }
}