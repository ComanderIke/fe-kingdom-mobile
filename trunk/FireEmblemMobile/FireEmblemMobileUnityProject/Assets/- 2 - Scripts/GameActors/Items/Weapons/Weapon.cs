using Assets.GameActors.Units;
using Assets.GameActors.Units.Humans;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.GameActors.Items.Weapons
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


        public override void Use(Human character)
        {
            base.Use(character);
        }

        public void Equip(Human character)
        {
            base.Use(character);
            character.Stats.AttackRanges.Clear();
            character.EquippedWeapon = this;
            foreach (int r in AttackRanges) character.Stats.AttackRanges.Add(r);
        }

        public void OnAttack(Unit attacker, Unit defender)
        {
            foreach (var mixin in WeaponMixins) mixin.OnAttack(attacker, defender);
        }
    }
}