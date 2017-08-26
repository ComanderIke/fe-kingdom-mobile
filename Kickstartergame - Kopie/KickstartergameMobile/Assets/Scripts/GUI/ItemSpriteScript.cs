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
		return new Potion("HealthPotion", "Restores 15 HP", 1, PotionType.HP, 15, Healthpotion_sprite, Healthpotion_go);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
