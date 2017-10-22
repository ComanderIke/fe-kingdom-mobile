using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField]
    GameObject bottomUI;
    [SerializeField]
    GameObject topUI;
    [SerializeField]
    Image faceSpriteLeft;
    [SerializeField]
    Image[] inventorySprites;
    [SerializeField]
    Text hpLeft;
    [SerializeField]
    Text atkLeft;
    [SerializeField]
    Text spdLeft;
    [SerializeField]
    Text defLeft;
    [SerializeField]
    Text accLeft;


    MainScript mainScript;
	// Use this for initialization
	void Start () {
        mainScript = FindObjectOfType<MainScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void HideBottomUI()
    {
        bottomUI.SetActive(false);
    }
    public void ShowBottomUI()
    {
        bottomUI.SetActive(true);
    }
    public void HideTopUI()
    {
        topUI.SetActive(false);
    }
    public void ShowTopUI(LivingObject c)
    {
        topUI.SetActive(true);

        hpLeft.text = c.HP+" / "+c.stats.maxHP;
        atkLeft.text = ""+c.stats.attack;
        spdLeft.text = "" + c.stats.speed;
        accLeft.text = "" + c.stats.accuracy;
        defLeft.text = "" + c.stats.defense;
        faceSpriteLeft.sprite = c.activeSpriteObject;
        foreach (Image i in inventorySprites)
        {
            i.enabled = false;
        }
        if (c.GetType() == typeof(Character))
        {
            Character character = (Character)c;
            for (int i = 0; i < character.items.Count; i++)
            {
                inventorySprites[i].sprite = character.items[i].sprite;
                inventorySprites[i].enabled = true;
            }
        }
    }
    public void EndTurnClicked()
    {
        mainScript.EndTurn();
    }
}
