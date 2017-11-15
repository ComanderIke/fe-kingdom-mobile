using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public delegate void AttackButtonCLicked();
    public static AttackButtonCLicked attacktButtonCLicked;

    [Header("Input Fields")]
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
    [SerializeField]
    public AttackUIController attackUIController;
    [SerializeField]
    GameObject reactUI;

    private MainScript mainScript;

	void Start () {
        mainScript = MainScript.GetInstance();
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

    public void ShowFightUI(LivingObject attacker, LivingObject defender)
    {
        attackUIController.Show(attacker,defender);
    }

    public void HideFightUI()
    {
        attackUIController.Hide() ;
    }

    public void ShowReactUI()
    {
        reactUI.SetActive(true);
    }

    public void HideReactUI()
    {
        reactUI.SetActive(false);
    }

    public void ShowTopUI(LivingObject c)
    {
        topUI.SetActive(true);

        hpLeft.text = c.Stats.HP+" / "+c.Stats.MaxHP;
        atkLeft.text = ""+c.Stats.Attack;
        spdLeft.text = "" + c.Stats.Speed;
        accLeft.text = "" + c.Stats.Accuracy;
        defLeft.text = "" + c.Stats.Defense;
        faceSpriteLeft.sprite = c.Sprite;
        foreach (Image i in inventorySprites)
        {
            i.enabled = false;
        }
        if (c.GetType() == typeof(Human))
        {
            Human character = (Human)c;
            for (int i = 0; i < character.Inventory.items.Count; i++)
            {
                inventorySprites[i].sprite = character.Inventory.items[i].Sprite;
                inventorySprites[i].enabled = true;
            }
        }
    }

    public void SkipFightButtonClicked()
    {
        attacktButtonCLicked();
    }

    public void EndTurnClicked()
    {
        EventContainer.endTurn();
    }
}
