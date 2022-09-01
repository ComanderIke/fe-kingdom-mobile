using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units.Numbers;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;

public class UIEventController : MonoBehaviour
{
    public Canvas canvas;
    public EventEncounterNode node;
    [HideInInspector]
    public Party party;
    private RandomEvent randomEvent;

    public TextMeshProUGUI description;
    public Transform layout;

    public GameObject textOptionPrefab;
    public GameObject itemOptionPrefab;
    public GameObject blessingOptionPrefab;
    public GameObject skillOptionPrefab;
    public GameObject fightOptionPrefab;
    public GameObject goldStoneOptionPrefab;


    // Start is called before the first frame update
    public void Show(EventEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.randomEvent = node.randomEvent;
        this.description.text = randomEvent.scenes[0].MainText;
        // if(instantiatedObjects==null)
        //     instantiatedObjects = new List<GameObject>();
        UpdateUI();
        //FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
        // for (int i=0; i<church.shopItems.Count; i++)
        // {
        //     var item = church.shopItems[i];
        //     shopItems[i].SetValues(new ShopItem(item.cost, item.Sprite, item.Description));
        // }
    }
    
    public void ContinueClicked()
    {
        canvas.enabled = false;
        FindObjectOfType<UICharacterViewController>().Hide();
        node.Continue();
    }

    public void OptionClicked(TextOptionController textOptionController)
    {
        Debug.Log("Option Clicked!");
    }

    public void UpdateUI()
    {
        layout.DeleteAllChildren();
        foreach (var textoption in randomEvent.scenes[0].textOptions)
        {
            var go=Instantiate(textOptionPrefab, layout);
            int stat = party.ActiveUnit.Stats.Attributes.GetFromIndex(textoption.StatIndex);
            string statText = stat+" "+Attributes.GetAsText(textoption.StatIndex);
            TextOptionState state = TextOptionState.Normal;
            if(stat < textoption.StatRequirement)
                state = TextOptionState.Impossible;
            else if(stat >= (textoption.StatRequirement+10))
                state = TextOptionState.High;
            go.GetComponent<TextOptionController>().Setup(textoption.Text,statText,state, this);
        }
    }
}
