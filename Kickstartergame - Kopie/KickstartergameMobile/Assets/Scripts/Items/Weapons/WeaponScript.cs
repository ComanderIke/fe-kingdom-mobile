﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using Assets.Scripts.Characters;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour {

    public Sprite recurveBowSprite;
    public Sprite bastardSwordSprite;
	public Sprite steelLanceSprite;
    public Sprite stilettoSprite;
	public Sprite warAxeSprite;

    public GameObject recurveBowGO;
    public GameObject bastardSwordGO;
    public GameObject stilettoGO;
	public GameObject warAxeGO;
	public GameObject steelLanceGO;
    [HideInInspector]
    public Weapon recurveBow;
    [HideInInspector]
    public Weapon steelLance;
    [HideInInspector]
    public Weapon bastardSword;
    [HideInInspector]
    public Weapon stiletto;
	[HideInInspector]
	public Weapon warAxe;

    public static WeaponCategory sword;
    public static WeaponCategory lance;
    public static WeaponCategory axe;
    public static WeaponCategory dagger;
    public static WeaponCategory bow;
    public static WeaponCategory magic;
    public static WeaponCategory staff;
    // Use this for initialization
    void Awake () {
        List<WeaponType> effective = new List<WeaponType>();
        sword = new WeaponCategory(WeaponType.Sword);
        lance = new WeaponCategory(WeaponType.Spear);
        axe = new WeaponCategory(WeaponType.Axe);
        dagger = new WeaponCategory(WeaponType.Dagger);
        bow = new WeaponCategory(WeaponType.Bow);
        magic = new WeaponCategory(WeaponType.Magic);
        staff = new WeaponCategory(WeaponType.Staff );
		recurveBow = new Weapon("Recurvebow"," axes", bow, 4, 110, 0, 0, 0, recurveBowGO, recurveBowSprite);
		steelLance = new Weapon("Steel Lance", " swords", lance, 8, 90, 0, 0, 0, steelLanceGO,steelLanceSprite);
        bastardSword = new Weapon("Bastardsword"," daggers",  sword, 10, 85, 0, 0, 0, bastardSwordGO, bastardSwordSprite);
		stiletto = new Weapon("Stiletto", " magic", dagger, 11, 70, 0, 0, 0, stilettoGO,stilettoSprite);
		warAxe = new Weapon("Waraxe",  " lances", axe, 4, 65, 0, 0, 0, warAxeGO, warAxeSprite);
    }

}
