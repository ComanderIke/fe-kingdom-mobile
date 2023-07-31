using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    
    public class Weapon : EquipableItem
    {
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] public WeaponAttributes[] WeaponAttributes;
        public int weaponLevel = 1;
        public int maxLevel = 3;
        
        public WeaponType WeaponType;
        public DamageType DamageType;
        [SerializeField] private Dictionary<EffectiveAgainstType, float> effectiveAgainst;
        

        public Weapon(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, int weaponLevel, int maxLevel,int[] attackRanges, WeaponAttributes[] weaponAttributes, WeaponType weaponType, DamageType damageType, Dictionary<EffectiveAgainstType, float> effectiveAgainst=null) : base(name, description, cost, rarity,maxStack,sprite)
        {
            this.weaponLevel = weaponLevel;
            this.maxLevel = maxLevel;
            this.AttackRanges = attackRanges;
            this.WeaponAttributes = weaponAttributes;
            this.WeaponType = weaponType;
            this.DamageType = damageType;
            this.effectiveAgainst = effectiveAgainst;
      
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

        public bool IsEffective(EffectiveAgainstType effectiveAgainstType)
        {
            return effectiveAgainst.ContainsKey(effectiveAgainstType)||WeaponType.IsEffective(effectiveAgainstType);
        }
        public float GetEffectiveCoefficient(EffectiveAgainstType effectiveAgainstType)
        {
            if (effectiveAgainst.ContainsKey(effectiveAgainstType))
            {
                return effectiveAgainst[effectiveAgainstType];
            }
            return WeaponType.GetEffectiveCoefficient(effectiveAgainstType);
        }

        public bool HasAdvantage(EffectiveAgainstType type)
        {
            return WeaponType.HasAdvantage(type);
        }
    }
}