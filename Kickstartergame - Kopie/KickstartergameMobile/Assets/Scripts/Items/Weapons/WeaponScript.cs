using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using Assets.Scripts.Characters;
using System.Collections.Generic;
using Assets.Scripts.Items.Weapons;

public class WeaponScript : MonoBehaviour {

    public Sprite basicBowSprite;
    public Sprite woodenSwordSprite;
	public Sprite woodenSpearSprite;
    public Sprite stilettoSprite;
	public Sprite woodenAxeSprite;

    [HideInInspector]
    public Weapon basicBow;
    [HideInInspector]
    public Weapon woodenSpear;
    [HideInInspector]
    public Weapon woodenSword;
	[HideInInspector]
	public Weapon woodenAxe;

    public static WeaponCategory sword;
    public static WeaponCategory lance;
    public static WeaponCategory axe;
    public static WeaponCategory dagger;
    public static WeaponCategory bow;
    public static WeaponCategory magic;
    public static WeaponCategory staff;
    // Use this for initialization
    void Awake () {
        sword = new WeaponCategory(WeaponType.Sword);
        lance = new WeaponCategory(WeaponType.Spear);
        axe = new WeaponCategory(WeaponType.Axe);
        dagger = new WeaponCategory(WeaponType.Dagger);
        bow = new WeaponCategory(WeaponType.Bow);
        magic = new WeaponCategory(WeaponType.Magic);
        staff = new WeaponCategory(WeaponType.Staff );
        List<int> meleeAttackRange = new List<int>();
        meleeAttackRange.Add(1);
        List<int> rangeAttackRange = new List<int>();
        rangeAttackRange.Add(1);
        rangeAttackRange.Add(2);
        rangeAttackRange.Add(3);
        rangeAttackRange.Add(4);
        woodenSword = new Weapon("Wooden Sword", "description", WeaponCategory.sword, 1, 5, 0, 0, meleeAttackRange, woodenSwordSprite);
        basicBow = new Weapon("Basic Bow", "description", WeaponCategory.bow, 1, -10, 0, 0, rangeAttackRange, basicBowSprite);
        woodenAxe = new Weapon("Wooden Axe", "description", WeaponCategory.axe, 3, -10, 0, 0, meleeAttackRange, woodenAxeSprite);
        woodenSpear = new Weapon("Wooden Spear", "description", WeaponCategory.spear, 2, 0, 0, 0, meleeAttackRange, woodenSpearSprite);

    }

}
