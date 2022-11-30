using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    public class Weapon : EquipableItem
    {
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] public WeaponAttributes[] WeaponAttributes;
        public int weaponLevel = 1;
        public int maxLevel = 3;
        
        public WeaponType WeaponType;
        public DamageType DamageType;
        [SerializeField] private Dictionary<EffectType, float> effectiveAgainst;

        public Weapon(string name, string description, int cost,int rarity, Sprite sprite, EquipmentSlotType slotType, int weaponLevel, int maxLevel,int[] attackRanges, WeaponAttributes[] weaponAttributes, WeaponType weaponType, DamageType damageType, Dictionary<EffectType, float> effectiveAgainst=null) : base(name, description, cost, rarity,sprite, slotType)
        {
            this.weaponLevel = weaponLevel;
            this.maxLevel = maxLevel;
            this.AttackRanges = attackRanges;
            this.WeaponAttributes = weaponAttributes;
            this.WeaponType = weaponType;
            this.DamageType = damageType;
            this.effectiveAgainst = effectiveAgainst;
      
        }
        public void OnEnable()
        {
            EquipmentSlotType = EquipmentSlotType.Weapon;
        }


        public int GetDamage()
        {
            return WeaponAttributes[weaponLevel-1].Dmg;
        }
        public int GetHit()
        {
            return WeaponAttributes[weaponLevel-1].Hit;
        }
        public int GetCrit()
        {
            return WeaponAttributes[weaponLevel-1].Crit;
        }
        public int GetWeight()
        {
            return WeaponAttributes[weaponLevel-1].Weight;
        }
        public override int GetUpgradeCost()
        {
            return WeaponAttributes[weaponLevel-1].upgradeGoldCost;
        }
        public override int GetUpgradeSmithingStoneCost()
        {
            return WeaponAttributes[weaponLevel-1].upgradeSmithingStoneCost;
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
                return WeaponAttributes[weaponLevel].Weight;
            else
                return 0;
        }

        public object GetUpgradeableCrit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel].Crit;
            else
                return 0;
        }

        public object GetUpgradeableHit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel].Hit;
            else
                return 0;
        }

        public object GetUpgradeableDmg()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponAttributes[weaponLevel].Dmg;
            else
                return 0;
        }

        public bool IsEffective(EffectType effectType)
        {
            return effectiveAgainst.ContainsKey(effectType)||WeaponType.IsEffective(effectType);
        }
        public float GetEffectiveCoefficient(EffectType effectType)
        {
            if (effectiveAgainst.ContainsKey(effectType))
            {
                return effectiveAgainst[effectType];
            }
            return WeaponType.GetEffectiveCoefficient(effectType);
        }
        
    }
}