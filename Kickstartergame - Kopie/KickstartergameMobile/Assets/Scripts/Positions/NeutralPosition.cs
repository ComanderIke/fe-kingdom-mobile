using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters.Classes;
using System.Collections.Generic;

public class NeutralPosition : MonoBehaviour {

    public CharacterClassType charType;
	public int startroom;
	public List<ItemEnum> inventory;
    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    public int GetX()
    {
        return (int)transform.localPosition.x;
    }
    public int GetZ()
    {
        return (int)transform.localPosition.z;
    }
	public List<Item>GetItems(){
		List<Item> items = new List<Item> ();
		WeaponScript ws = GameObject.Find ("RessourceScript").GetComponent<WeaponScript> ();
		ItemSpriteScript iss = GameObject.Find ("RessourceScript").GetComponent<ItemSpriteScript> ();
		foreach (ItemEnum i in inventory) {
			switch (i) {
			case ItemEnum.Recurvebow:
				items.Add (ws.recurveBow);
				break;
			case ItemEnum.Arkum:items.Add (ws.arkum);
				break;
			case ItemEnum.Valkyrie:items.Add (ws.valkyrie);
				break;
			case ItemEnum.Dragonstinger:items.Add (ws.dragonstinger);
				break;
			case ItemEnum.SpearPrincess:items.Add (ws.spearPrincess);
				break;
			case ItemEnum.SteelLance:items.Add (ws.steelLance);
				break;
			case ItemEnum.Knightsword:items.Add (ws.knightSword);
				break;
			case ItemEnum.Avenger:items.Add (ws.avenger);
				break;
			case ItemEnum.Bastardsword:items.Add (ws.bastardSword);
				break;
			case ItemEnum.Deathbringer:items.Add (ws.deathbringer);
				break;
			case ItemEnum.Stiletto:items.Add (ws.stiletto);
				break;
			case ItemEnum.HuntingKnife:items.Add (ws.huntingKnife);
				break;
			case ItemEnum.Ignis:items.Add (ws.ignis);
				break;
			case ItemEnum.Ventus:items.Add (ws.ventus);
				break;
			case ItemEnum.ThanatosBreath:items.Add (ws.thanatosBreath);
				break;
			case ItemEnum.Waraxe:items.Add (ws.warAxe);
				break;
			case ItemEnum.Slasher:items.Add (ws.slasher);
				break;
			case ItemEnum.Healthpotion:items.Add (iss.CreateHealthPotion());
				break;
			case ItemEnum.Healthcrystal:items.Add (iss.CreateHealthCrystal());
				break;
			case ItemEnum.Manacrystal:items.Add (iss.CreateManaCrystal());
				break;
			case ItemEnum.Experiencecrystal:items.Add (iss.CreateExpCrystal());
				break;
			case ItemEnum.DoorKey:items.Add (iss.CreateKey());
				break;
			}
		}
		return items;
	}

    // Update is called once per frame
    void Update () {
	
	}
}
