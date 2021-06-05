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
    public class Weapon : Item
    {
        
        [Range(1, 4)] public int[] AttackRanges;


        [Header("WeaponAttributes")] public int Dmg;
        public int Weight;

        public List<WeaponMixin> WeaponMixins;
        public WeaponType WeaponType;
        public DamageType DamageType;

        public override void Use(Human character)
        {
            base.Use(character);
        }

      

        public void OnAttack(Unit attacker, Unit defender)
        {
            foreach (var mixin in WeaponMixins) mixin.OnAttack(attacker, defender);
        }
    }
}