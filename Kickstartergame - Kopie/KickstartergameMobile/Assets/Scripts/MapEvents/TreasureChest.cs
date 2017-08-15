using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using UnityEngine.UI;
using Assets.Scripts.Characters;

[System.Serializable]
public enum ItemType
{
    Potion,
    Weapon
};
[System.Serializable]
public enum Itemlist{

	Healthpotion,
	IronSword,
	IronSpear,
	IronBow,
	MagicBook,
	Staff,
	IronDagger,
	ThanatosBreath,
	DragonStinger,
	Valkyrie,
	Avenger,
	Ventus,
	SteelLance,
	Slasher,
	Arkum

}
public class TreasureChest : MonoBehaviour {

    public Item item;
    public bool opened = false;
    public GameObject MessageDialog;
	public Itemlist item1;
	public Itemlist item2;
	public Itemlist item3;
	public Itemlist item4;

    public bool isAnimationPlaying = false;
    private bool init = true;
    private bool inventoryFull = false;
    public bool bigChest = false;
    // Use this for initialization
    void Start () {
        

        //if(itemType == ItemType.Potion)
        //item = new Potion(itemName, useage, potionType, effect);
        //if (itemType == ItemType.Weapon)
        //    item = new Weapon(itemName, weaponType, dmg, hit, crit, price, range, null);

    }
    float time = 0;
    public virtual void Open(Character character)
    {
        inventoryFull = false;
        if (!opened )
        {
			if (character.characterClassType == Assets.Scripts.Characters.Classes.CharacterClassType.Archer) {
				switch (item1) {
				case Itemlist.Healthpotion:
					item = new Potion ("Health Potion", "description", 1, PotionType.HP, 10, null, null, null, new GameObject (), new GameObject (), false);
					break;			
				case Itemlist.Valkyrie:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().valkyrie;
					break;
				case Itemlist.Arkum:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().arkum;
					break;
				}
			} else if (character.characterClassType == Assets.Scripts.Characters.Classes.CharacterClassType.Mage) {
				switch (item2) {
				case Itemlist.Healthpotion:
					item = new Potion ("Health Potion", "description", 1, PotionType.HP, 10, null, null, null, new GameObject (), new GameObject (), false);
					break;
				case Itemlist.Ventus:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().ventus;
					break;
				case Itemlist.ThanatosBreath:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().thanatosBreath;
					break;
				}
			} else if (character.characterClassType == Assets.Scripts.Characters.Classes.CharacterClassType.SwordFighter) {
				switch (item3) {
				case Itemlist.Healthpotion:
					item = new Potion ("Health Potion", "description", 1, PotionType.HP, 10, null, null, null, new GameObject (), new GameObject (), false);
					break;
				case Itemlist.Avenger:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().avenger;
					break;
				case Itemlist.Slasher:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().slasher;
					break;
				}
			} else if (character.characterClassType == Assets.Scripts.Characters.Classes.CharacterClassType.Hellebardier) {
				switch (item4) {
				case Itemlist.Healthpotion:
					item = new Potion ("Health Potion", "description", 1, PotionType.HP, 10, null, null, null, new GameObject (), new GameObject (), false);
					break;
				case Itemlist.DragonStinger:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().dragonstinger;
					break;
				case Itemlist.SteelLance:
					item = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ().steelLance;
					break;
				}
			}
            GetComponentInChildren<Animator>().SetTrigger("Open");
            opened = true;
            //createRandomItem(character);
            character.addItem(item);
            isAnimationPlaying = true;
            MessageDialog.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            MessageDialog.GetComponentInChildren<Text>().text = "Obtained ";
			MessageDialog.GetComponentsInChildren<Image> ()[1].sprite = item.sprite_hovered;
            GetComponent<AudioSource>().Play();
            Debug.Log("Obtained " + item.Name + "!");
			character.gameObject.GetComponent<CharacterScript> ().PlayOpenChestAnimation ();
			MainScript.GetInstance ().gridScript.HideMovement ();
        }
        
    }
    public virtual void createRandomItem(Character character)
    {
        if (!bigChest)
        {
            switch (Random.Range(1, 4))
            {
                case 1: item = GameObject.Find("RessourceScript").GetComponent<ItemSpriteScript>().CreateHealthPotion(); break;
                case 2: item = GameObject.Find("RessourceScript").GetComponent<ItemSpriteScript>().CreateCDRPotion(); break;
                case 3: item = GameObject.Find("RessourceScript").GetComponent<ItemSpriteScript>().CreateBoots(); break;
                case 4: item = GameObject.Find("RessourceScript").GetComponent<ItemSpriteScript>().CreateDoubleAttack(); break;
            }
        }
        
    }
    public int getX()
    {
        return (int)(transform.position.x-0.5f);
    }
    public int getZ()
    {
        return (int)(transform.position.z-0.5f);
    }

    // Update is called once per frame
    void Update () {
        if (init)
        {
            init = false;
            //SendMessageUpwards("PlaceChest", this);
        }
        if (isAnimationPlaying)
        {
            time += Time.deltaTime;
            //Animation
        }
        if (inventoryFull)
        {
            time += Time.deltaTime;
        }
        if (opened)
        {
            time += Time.deltaTime;
        }
        if (time > 2.5f)
        {
            MessageDialog.transform.position = new Vector3(-500, Screen.height / 2, 0);
            time = 0;
            inventoryFull = false;
            GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().canEndTurn = true;
        }
    }
}
