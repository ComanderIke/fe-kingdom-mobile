using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Weapons/Weapon", fileName = "Weapon")]
    public class WeaponBP :EquipableItemBP
    {
        
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] 
        public WeaponAttributes[] WeaponAttributes;
        public int weaponLevel = 1;
        public int maxLevel = 3;
        
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
            return new Weapon(name, description, cost, rarity,maxStack,sprite, equipmentSlotType, weaponLevel, maxLevel, AttackRanges,
                WeaponAttributes, WeaponType, DamageType, effectiveness);
        }
    }
}