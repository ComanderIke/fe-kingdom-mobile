using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : MonoBehaviour {

    [SerializeField]
    Image faceSprite;
    [SerializeField]
    Image[] inventorySprites;
    [SerializeField]
    TextMeshProUGUI hp;
    [SerializeField]
    TextMeshProUGUI atk;
    [SerializeField]
    TextMeshProUGUI weaponAtk;
    [SerializeField]
    TextMeshProUGUI spd;
    [SerializeField]
    TextMeshProUGUI def;
    [SerializeField]
    TextMeshProUGUI acc;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Show(Unit c)
    {
        gameObject.SetActive(true);
        hp.text = c.HP + " / " + c.Stats.MaxHP;
        atk.text = "" + c.Stats.Attack;
        spd.text = "" + c.Stats.Speed;
        acc.text = "" + c.Stats.Accuracy;
        def.text = "" + c.Stats.Defense;
        weaponAtk.text = "";

        //faceSpriteLeft.sprite = c.Sprite;
        foreach (Image i in inventorySprites)
        {
            i.enabled = false;
        }
        if (c.GetType() == typeof(Human))
        {
            Human character = (Human)c;
            if (character.EquipedWeapon != null)
                weaponAtk.text = "+" + character.EquipedWeapon.Dmg;
            for (int i = 0; i < character.Inventory.items.Count; i++)
            {
                inventorySprites[i].sprite = character.Inventory.items[i].Sprite;
                inventorySprites[i].enabled = true;
            }
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
