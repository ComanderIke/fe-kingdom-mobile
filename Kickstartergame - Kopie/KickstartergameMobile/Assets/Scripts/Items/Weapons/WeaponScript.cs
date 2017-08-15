using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using Assets.Scripts.Characters;
using System.Collections.Generic;

public class WeaponScript : MonoBehaviour {

    public Sprite recurveBowSprite;
    public Sprite recurveBowSprite_hovered;
    public Sprite recurveBowSprite_pressed;
	public Sprite recurveBowSprite_attack;
	public Sprite recurveBowSprite_attack2;
    public Sprite arkumSprite;
    public Sprite arkumSprite_hovered;
    public Sprite arkumSprite_pressed;
	public Sprite arkumSprite_attack;
	public Sprite arkumSprite_attack2;
    public Sprite valkyrieSprite;
    public Sprite valkyrieSprite_hovered;
    public Sprite valkyrieSprite_pressed;
	public Sprite valkyrieSprite_attack;
	public Sprite valkyrieSprite_attack2;

    public Sprite knightSwordSprite;
    public Sprite knightSwordSprite_hovered;
    public Sprite knightSwordSprite_pressed;
	public Sprite knightSwordSprite_attack;
	public Sprite knightSwordSprite_attack2;
    public Sprite bastardSwordSprite;
    public Sprite bastardSwordSprite_hovered;
    public Sprite bastardSwordSprite_pressed;
	public Sprite bastardSwordSprite_attack;
	public Sprite bastardSwordSprite_attack2;
    public Sprite avengerSprite;
    public Sprite avengerSprite_hovered;
    public Sprite avengerSprite_pressed;
	public Sprite avengerSprite_attack;
	public Sprite avengerSprite_attack2;

	public Sprite dragonstingerSprite;
	public Sprite dragonstingerSprite_hovered;
	public Sprite dragonstingerSprite_pressed;
	public Sprite dragonstingerSprite_attack;
	public Sprite dragonstingerSprite_attack2;
	public Sprite steelLanceSprite;
	public Sprite steelLanceSprite_hovered;
	public Sprite steelLanceSprite_pressed;
	public Sprite steelLanceSprite_attack;
	public Sprite steelLanceSprite_attack2;
	public Sprite princessSpearSprite;
	public Sprite princessSpearSprite_hovered;
	public Sprite princessSpearSprite_pressed;
	public Sprite princessSpearSprite_attack;
	public Sprite princessSpearSprite_attack2;

    public Sprite stilettoSprite;
    public Sprite stilettoSprite_hovered;
    public Sprite stilettoSprite_pressed;
	public Sprite stilettoSprite_attack;
	public Sprite stilettoSprite_attack2;
    public Sprite deathbringerSprite;
    public Sprite deathbringerSprite_hovered;
    public Sprite deathbringerSprite_pressed;
	public Sprite deathbringerSprite_attack;
	public Sprite deathbringerSprite_attack2;
	public Sprite huntingKnifeSprite;
	public Sprite huntingKnifeSprite_hovered;
	public Sprite huntingKnifeSprite_pressed;
	public Sprite huntingKnifeSprite_attack;
	public Sprite huntingKnifeSprite_attack2;

    public Sprite ignisSprite;
    public Sprite ignisSprite_hovered;
    public Sprite ignisSprite_pressed;
	public Sprite ignisSprite_attack;
	public Sprite ignisSprite_attack2;
    public Sprite ventusSprite;
    public Sprite ventusSprite_hovered;
    public Sprite ventusSprite_pressed;
	public Sprite ventusSprite_attack;
	public Sprite ventusSprite_attack2;
    public Sprite thanatosBreathSprite;
    public Sprite thanatosBreathSprite_hovered;
    public Sprite thanatosBreathSprite_pressed;
	public Sprite thanatosBreathSprite_attack;
	public Sprite thanatosBreathSprite_attack2;

	public Sprite sanctusSprite;
	public Sprite sanctusSprite_hovered;
	public Sprite sanctusSprite_pressed;
	public Sprite sanctusSprite_attack;
	public Sprite sanctusSprite_attack2;
	public Sprite medicusSprite;
	public Sprite medicusSprite_hovered;
	public Sprite medicusSprite_pressed;
	public Sprite medicusSprite_attack;
	public Sprite medicusSprite_attack2;
	public Sprite auroraSprite;
	public Sprite auroraSprite_hovered;
	public Sprite auroraSprite_pressed;
	public Sprite auroraSprite_attack;
	public Sprite auroraSprite_attack2;

	public Sprite warAxeSprite;
	public Sprite warAxeSprite_hovered;
	public Sprite warAxeSprite_pressed;
	public Sprite warAxeSprite_attack;
	public Sprite warAxeSprite_attack2;
	public Sprite slasherSprite;
	public Sprite slasherSprite_hovered;
	public Sprite slasherSprite_pressed;
	public Sprite slasherSprite_attack;
	public Sprite slasherSprite_attack2;

    public GameObject recurveBowGO;
    public GameObject arkumGO;
    public GameObject valkyrieGO;

	public GameObject knightSwordGO;
	public GameObject knightSwordGO3D;
    public GameObject bastardSwordGO;
    public GameObject avengerGO;

    public GameObject stilettoGO;
    public GameObject deathbringerGO;
	public GameObject huntingKnifeGO;
	public GameObject huntingKnifeGO3D;

	public GameObject warAxeGO;
	public GameObject slasherGO;

	public GameObject princessSpearGO;
	public GameObject dragonstingerGO;
	public GameObject steelLanceGO;

    public GameObject ignisGO;
    public GameObject ventusGO;
    public GameObject thanatosBreathGO;

	public GameObject sanctusGO;
	public GameObject medicusGO;
	public GameObject auroraGO;


	public GameObject arrowPrefab;
	public GameObject arrowHolding;
	public GameObject firearrowPrefab;
	public GameObject frostarrowPrefab;
    [HideInInspector]
    public Weapon recurveBow;
    [HideInInspector]
    public Weapon arkum;
    [HideInInspector]
    public Weapon valkyrie;

    [HideInInspector]
    public Weapon dragonstinger;
    [HideInInspector]
    public Weapon spearPrincess;
    [HideInInspector]
    public Weapon steelLance;

	[HideInInspector]
	public Weapon knightSword;
    [HideInInspector]
    public Weapon avenger;
    [HideInInspector]
    public Weapon bastardSword;

    [HideInInspector]
    public Weapon deathbringer;
    [HideInInspector]
    public Weapon stiletto;
	[HideInInspector]
	public Weapon huntingKnife;

	[HideInInspector]
	public Weapon sanctus;
	[HideInInspector]
	public Weapon medicus;
	[HideInInspector]
	public Weapon aurora;

    [HideInInspector]
    public Weapon ignis;
    [HideInInspector]
    public Weapon ventus;
    [HideInInspector]
    public Weapon thanatosBreath;

	[HideInInspector]
	public Weapon warAxe;
	[HideInInspector]
	public Weapon slasher;

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
       // effective.Add(WeaponType.Dagger);
        //effective.Add(WeaponType.Axe);
        sword = new WeaponCategory(WeaponType.Sword);
       // effective = new List<WeaponType>();
        //effective.Add(WeaponType.Sword);
        //effective.Add(WeaponType.Bow);
        lance = new WeaponCategory(WeaponType.Spear);
       // effective = new List<WeaponType>();
       // effective.Add(WeaponType.Spear);
       // effective.Add(WeaponType.Magic);
        axe = new WeaponCategory(WeaponType.Axe);
       // effective = new List<WeaponType>();
        //effective.Add(WeaponType.Spear);
      //  effective.Add(WeaponType.Magic);
        dagger = new WeaponCategory(WeaponType.Dagger);

        bow = new WeaponCategory(WeaponType.Bow);

        magic = new WeaponCategory(WeaponType.Magic);
        staff = new WeaponCategory(WeaponType.Staff );
		recurveBow = new Weapon("Recurvebow"," axes", bow, 4, 110, 0, 0, 0, recurveBowGO, knightSwordGO3D, recurveBowSprite, recurveBowSprite_hovered, recurveBowSprite_pressed, recurveBowSprite_attack,recurveBowSprite_attack2, false);
		arkum = new Weapon("Arkum"," axes",  bow, 8, 100, 0, 0, 0, arkumGO, knightSwordGO3D, arkumSprite, arkumSprite_hovered, arkumSprite_pressed, arkumSprite_attack,arkumSprite_attack2,false);
		valkyrie = new Weapon ("Valkyrie"," axes", bow, 12, 90, 0, 0, 0, valkyrieGO, knightSwordGO3D, valkyrieSprite, valkyrieSprite_hovered, valkyrieSprite_pressed, valkyrieSprite_attack,valkyrieSprite_attack2, false);

		dragonstinger = new Weapon("Dragonstinger"," swords", lance, 15, 65, 0, 0, 0, dragonstingerGO, knightSwordGO3D,dragonstingerSprite, dragonstingerSprite_hovered, dragonstingerSprite_pressed, dragonstingerSprite_attack, dragonstingerSprite_attack2, false);
		spearPrincess = new Weapon("SpearPrincess", " swords", lance, 5, 80, 0, 0, 0, princessSpearGO,knightSwordGO3D, princessSpearSprite, princessSpearSprite_hovered, princessSpearSprite_pressed, princessSpearSprite_attack,princessSpearSprite_attack2,false);
		steelLance = new Weapon("Steel Lance", " swords", lance, 8, 90, 0, 0, 0, steelLanceGO, knightSwordGO3D,steelLanceSprite, steelLanceSprite_hovered, steelLanceSprite_pressed, steelLanceSprite_attack, steelLanceSprite_attack2,false);

		knightSword = new Weapon("Knightsword"," daggers",  sword, 4, 95, 0, 0, 0, knightSwordGO, knightSwordGO3D,knightSwordSprite, knightSwordSprite_hovered, knightSwordSprite_pressed, knightSwordSprite_attack,knightSwordSprite_attack2, true);
		avenger = new Weapon("Avenger"," daggers",  sword,14, 70, 0, 0, 0, avengerGO,knightSwordGO3D, avengerSprite, avengerSprite_hovered, avengerSprite_pressed, avengerSprite_attack, avengerSprite_attack2,false);
		bastardSword = new Weapon("Bastardsword"," daggers",  sword, 10, 85, 0, 0, 0, bastardSwordGO,knightSwordGO3D, bastardSwordSprite, bastardSwordSprite_hovered, bastardSwordSprite_pressed, bastardSwordSprite_attack,bastardSwordSprite_attack2, false);

		deathbringer = new Weapon("Deathbringer", " magic", dagger, 6, 95, 0, 0, 0, deathbringerGO, knightSwordGO3D,deathbringerSprite, deathbringerSprite_hovered, deathbringerSprite_pressed, deathbringerSprite_attack, deathbringerSprite_attack2, false);
		stiletto = new Weapon("Stiletto", " magic", dagger, 11, 70, 0, 0, 0, stilettoGO, knightSwordGO3D,stilettoSprite, stilettoSprite_hovered, stilettoSprite_pressed, stilettoSprite_attack, stilettoSprite_attack2, false);
		huntingKnife = new Weapon("Hunting knife", " magic", dagger, 11, 70, 0, 0, 0, huntingKnifeGO, huntingKnifeGO3D,huntingKnifeSprite, huntingKnifeSprite_hovered,huntingKnifeSprite_pressed, huntingKnifeSprite_attack, huntingKnifeSprite_attack2, false);

		sanctus = new Weapon("Sanctus", "restores 5 + users Magic HP", staff, 5, 80, 0, 0, 0, sanctusGO, knightSwordGO3D,sanctusSprite, sanctusSprite_hovered, sanctusSprite_pressed, sanctusSprite_attack, sanctusSprite_attack2, false);
		medicus = new Weapon("Medicus", "restores 10 + users Magic HP", staff, 5, 80, 0, 0, 0, medicusGO, knightSwordGO3D,medicusSprite, medicusSprite_hovered, medicusSprite_pressed, medicusSprite_attack, medicusSprite_attack2, false);
		aurora = new Weapon("Aurora", "restores all HP and Mana", staff, 5, 80, 0, 0, 0, auroraGO, knightSwordGO3D,auroraSprite, auroraSprite_hovered, auroraSprite_pressed, auroraSprite_attack,  auroraSprite_attack2,false);

		ignis = new Weapon("Ignis", " bows", magic, 2, 80, 0, 0, 0, ignisGO, knightSwordGO3D,ignisSprite, ignisSprite_hovered, ignisSprite_pressed, ignisSprite_attack,ignisSprite_attack2,false);
		ventus = new Weapon("Ventus", " bows", magic, 3, 100, 0, 0, 0, ventusGO,knightSwordGO3D, ventusSprite, ventusSprite_hovered, ventusSprite_pressed,ventusSprite_attack,ventusSprite_attack2, false);
		thanatosBreath = new Weapon("ThanatosBreath"," bows", magic, 16, 60, 0, 0, 0, thanatosBreathGO,knightSwordGO3D, thanatosBreathSprite, thanatosBreathSprite_hovered, thanatosBreathSprite_pressed, thanatosBreathSprite_attack,thanatosBreathSprite_attack2,false);

		warAxe = new Weapon("Waraxe",  " lances", axe, 4, 65, 0, 0, 0, warAxeGO,knightSwordGO3D, warAxeSprite, warAxeSprite_hovered, warAxeSprite_pressed, warAxeSprite_attack,warAxeSprite_attack2, false);
		slasher = new Weapon("Slasher", " lances", axe, 13, 60, 0, 0, 0, slasherGO,knightSwordGO3D,slasherSprite, slasherSprite_hovered, slasherSprite_pressed, slasherSprite_attack,slasherSprite_attack2,false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
