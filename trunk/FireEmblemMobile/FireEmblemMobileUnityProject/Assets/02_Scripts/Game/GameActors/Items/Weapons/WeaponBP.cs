using System;
using System.Collections.Generic;
using Game.GameActors.Items.Relics;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.UnitType;
using Game.Systems;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Weapons/Weapon", fileName = "Weapon")]
    public class WeaponBP :EquipableItemBP
    {
        
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] 
        public WeaponAttributes WeaponAttributes;
        public UpgradeAttributes[] UpgradeAttributes;
        public int weaponLevel = 1;
        public int maxLevel = 3;
        public SkillBp skillBp;
        public WeaponType WeaponType;
        public DamageType DamageType;
        public List<EffectiveAgainstType> effectiveAgainst;
        public List<float> effectiveAgainstCoefficients;

        public override Item Create()
        {
            Dictionary<EffectiveAgainstType, float> effectiveness = new Dictionary<EffectiveAgainstType, float>();
            for (int i = 0; i < effectiveAgainst.Count; i++)
            {
                effectiveness.Add(effectiveAgainst[i], effectiveAgainstCoefficients[i]);
            }
            return new Weapon(name, description, cost, rarity,maxStack,sprite, weaponLevel, maxLevel, AttackRanges,
                WeaponAttributes,UpgradeAttributes, WeaponType, DamageType, skillBp==null?null:skillBp.Create(),effectiveness );
        }
    }
}