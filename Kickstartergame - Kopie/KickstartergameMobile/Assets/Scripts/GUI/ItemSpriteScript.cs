using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class ItemSpriteScript : MonoBehaviour {

    public Sprite Healthpotion_sprite;
    public Sprite DoubleAttackPotion_sprite;
    public Sprite CDR_sprite;
    public Sprite Healthpotion_hovered;
    public Sprite DoubleAttackPotion_hovered;
    public Sprite CDR_hovered;
    public Sprite Healthpotion_pressed;
    public Sprite DoubleAttackPotion_pressed;
    public Sprite CDR_pressed;
    public Sprite Boots_sprite;
    public Sprite Boots_hovered;
    public Sprite Boots_pressed;
	public Sprite key_sprite;
	public Sprite key_hovered;
	public Sprite key_pressed;
	public GameObject Healthpotion_go;
	public GameObject CDR_go;
	public GameObject Boots_go;
	public GameObject DoubleAttackPotion_go;
	public GameObject key_go;
	public GameObject key_go3d;
	public GameObject healthcrystal_go;
	public GameObject healthcrystal_go3d;
	public Sprite healthcrystal_sprite;
	public Sprite healthcrystal_sprite_hovered;
	public Sprite healthcrystal_sprite_pressed;
	public GameObject manacrystal_go;
	public GameObject manacrystal_go3d;
	public Sprite manacrystal_sprite;
	public Sprite manacrystal_sprite_hovered;
	public Sprite manacrystal_sprite_pressed;
	public GameObject expcrystal_go;
	public GameObject expcrystal_go3d;
	public Sprite expcrystal_sprite;
	public Sprite expcrystal_sprite_hovered;
	public Sprite expcrystal_sprite_pressed;

    // Use this for initialization
    // Use this for initialization
    void Start () {
    }
	
    public Potion CreateHealthPotion()
    {
		return new Potion("HealthPotion", "Restores 15 HP", 1, PotionType.HP, 15, Healthpotion_sprite, Healthpotion_hovered, Healthpotion_pressed, Healthpotion_go,Healthpotion_go, false);
    }
    public Potion CreateCDRPotion()
    {
		return new Potion("CD-Reduction Potion", "this is a healthpotion", 1, PotionType.CDR, 10, CDR_sprite, CDR_hovered, CDR_pressed, CDR_go ,CDR_go,false);
    }
    public Potion CreateBoots()
    {
		return new Potion("Boots", "this is a healthpotion", 1, PotionType.MOV, 10, Boots_sprite, Boots_hovered, Boots_pressed, Boots_go,Boots_go, false);
    }
    public Potion CreateDoubleAttack()
    {
		return new Potion("Double Attack","this is a healthpotion",  1, PotionType.DoubleAttack, 10, DoubleAttackPotion_sprite, DoubleAttackPotion_hovered, DoubleAttackPotion_pressed, DoubleAttackPotion_go, DoubleAttackPotion_go, false);
    }
	public Potion CreateHealthCrystal(){
		return new Potion ("Healthcrystal", "Restores 10 HP", 1, PotionType.HP, 10, healthcrystal_sprite, healthcrystal_sprite_hovered, healthcrystal_sprite_pressed, healthcrystal_go, healthcrystal_go3d, false);
	}
	public Potion CreateManaCrystal(){
		return new Potion ("Manacrystal", "Restores 10 Mana", 1, PotionType.HP, 10, manacrystal_sprite, manacrystal_sprite_hovered, manacrystal_sprite_pressed, manacrystal_go, manacrystal_go3d, false);
	}
	public Potion CreateExpCrystal(){
		return new Potion ("Experiencecrystal", "Grants 50 EXP", 1, PotionType.EXP, 50, expcrystal_sprite, expcrystal_sprite_hovered, expcrystal_sprite_pressed, expcrystal_go, expcrystal_go3d, false);
	}
	public DoorKey CreateKey(){
		return new DoorKey ("Door Key", "Opens a closed door", 1,key_sprite,key_hovered, key_pressed, key_go, key_go3d, true);
	}
    // Update is called once per frame
    void Update () {
	
	}
}
