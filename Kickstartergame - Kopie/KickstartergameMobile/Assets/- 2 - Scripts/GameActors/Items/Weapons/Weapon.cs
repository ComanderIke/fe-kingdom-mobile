using Assets.Scripts.Characters;
using Assets.Scripts.Items.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "GameData/Weapons/Weapon", fileName = "Weapon")]
public class Weapon : Item
{
    [Header("WeaponAttribtues")]
    public int Dmg;
    public int Hit;
    [Range(1, 4)]
    public int[] AttackRanges;
    public int Crit;
    public WeaponType WeaponType;
    public List<WeaponMixin> weaponMixins;

    public override void Use(Human character)
    {
        base.Use(character);
    }
    public void Equip(Human character)
    {
        base.Use(character);
        character.Stats.AttackRanges.Clear();
        character.EquipedWeapon = this;
        foreach (int r in AttackRanges)
        {
            character.Stats.AttackRanges.Add(r);
        }
    }
    public void OnAttack(Unit attacker, Unit defender)
    {
        foreach (WeaponMixin mixin in weaponMixins)
        {
            mixin.OnAttack(attacker, defender);
        }
    }

}

