using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items.Relics;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.UnitType;
using Game.GUI.EncounterUI.Smithy;
using Game.Systems;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    
    public class Weapon : EquipableItem
    {
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] public UpgradeAttributes[] WeaponUpgrades;
        public WeaponAttributes WeaponAttributes;
        public int weaponLevel = 1;
        public int powerUpgLvl = 0;
        public int accUpgLvl = 0;
        public int critUpgLvl = 0;
        public int specialUpgLvl = 0;
        public int maxLevel = 3;
        
        public WeaponType WeaponType;
        public DamageType DamageType;
        [SerializeField] private Dictionary<EffectiveAgainstType, float> effectiveAgainst;

        public Weapon(string name, string description, int cost,int rarity, int maxStack,Sprite sprite, int weaponLevel, int maxLevel,int[] attackRanges,
            WeaponAttributes weaponAttributes,UpgradeAttributes[] weaponUpgrades, WeaponType weaponType, DamageType damageType, 
            Skill skill, float strScaling, float dexScaling,float intScaling,Dictionary<EffectiveAgainstType,  float> effectiveAgainst=null) : base(name, description, cost, rarity,maxStack,sprite, skill)
        {
            this.weaponLevel = weaponLevel;
            this.maxLevel = maxLevel;
            this.AttackRanges = attackRanges;
            this.WeaponAttributes = weaponAttributes;
            this.WeaponType = weaponType;
            this.DamageType = damageType;
            this.effectiveAgainst = effectiveAgainst;
            this.WeaponUpgrades = weaponUpgrades;
            this.strScaling = strScaling;
            this.dexScaling = dexScaling;
            this.intScaling = intScaling;

        }

        public void Bind(Unit user)
        {
            if (Skill != null)
            {
                Skill.BindSkill(user);
            }
        }
        public void Unbind(Unit user)
        {
            if (Skill != null)
            {
                Skill.UnbindSkill(user);
            }
        }
        public int GetDamage()
        {
            return WeaponAttributes.Dmg;
        }
        public int GetHit()
        {
            return WeaponAttributes.Hit;
        }
        public int GetCrit()
        {
            return WeaponAttributes.Crit;
        }
        public int GetWeight()
        {
            return WeaponAttributes.Weight;
        }

        public string GetRank()
        {
            switch (weaponLevel-1)
            {
                case 0: return "C";
                case 1: return "B";
                case 2: return "A";
                case 3: return "S";
            }

            return "U";
        }
   
        public void Upgrade(WeaponUpgradeMode mode)
        {
            weaponLevel++;
            switch (mode)
            {
                case WeaponUpgradeMode.Power:
                    WeaponAttributes.Dmg += WeaponUpgrades[powerUpgLvl].Dmg;
                    WeaponAttributes.Weight += WeaponUpgrades[powerUpgLvl].weightCostPower;
                    powerUpgLvl++; break;
                case WeaponUpgradeMode.Accuracy:
                    WeaponAttributes.Hit += WeaponUpgrades[accUpgLvl].Hit;
                    WeaponAttributes.Weight += WeaponUpgrades[accUpgLvl].weightCostAccuracy;
                    accUpgLvl++; break;
                case WeaponUpgradeMode.Critical:
                    WeaponAttributes.Crit += WeaponUpgrades[critUpgLvl].Crit;
                    WeaponAttributes.Weight += WeaponUpgrades[critUpgLvl].weightCostCrit;
                    critUpgLvl++; break;
                case WeaponUpgradeMode.Special:
                    WeaponAttributes.effect = WeaponUpgrades[specialUpgLvl].effect;
                    WeaponAttributes.Weight += WeaponUpgrades[specialUpgLvl].weightCostSpecial;
                    specialUpgLvl++;
                    Debug.Log("TODO SPECIAL UPGRADE"); break;
            }
            if (weaponLevel > maxLevel)
                weaponLevel = maxLevel;
        }

       public int GetUpgradeableCrit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[critUpgLvl].Crit;
            else
                return 0;
        }

        public int GetUpgradeableHit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[accUpgLvl].Hit;
            else
                return 0;
        }

        public int GetUpgradeableDmg()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[powerUpgLvl].Dmg;
            else
                return 0;
        }
        public int GetUpgradeableWeightPower()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[powerUpgLvl].weightCostPower;
            else
                return 0;
        }
        public int GetUpgradeableWeightAccuracy()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[accUpgLvl].weightCostAccuracy;
            else
                return 0;
        }
        public int GetUpgradeableWeightCrit()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[critUpgLvl].weightCostCrit;
            else
                return 0;
        }
        public int GetUpgradeableWeightSpecial()
        {
            if (weaponLevel + 1 <= maxLevel)
                return WeaponUpgrades[specialUpgLvl].weightCostSpecial;
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

        private float dexScaling = 0;
        private float strScaling = 1;
        private float intScaling = 0;

        public float GetDexScaling()
        {
            return dexScaling;
        }
        public float GetStrScaling()
        {
            return strScaling;
        }
        public float GetIntScaling()
        {
            return intScaling;
        }

        public void SetDexScaling(float dexScaling)
        {
            this.dexScaling = dexScaling;
        }
        public void SetStrScaling(float strScaling)
        {
            this.strScaling = strScaling;
        }
        public void SetIntScaling(float intScaling)
        {
            this.intScaling = intScaling;
        }
    }
}