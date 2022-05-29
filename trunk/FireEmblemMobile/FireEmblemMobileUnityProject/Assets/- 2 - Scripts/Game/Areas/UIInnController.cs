using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInnController : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas canvas;
    [HideInInspector]
    public Party party;
   // public TextMeshProUGUI personName;
    //public TextMeshProUGUI talkText;

    private Inn inn;
    
    public List<UIShopItemController> shopItems;
    public UIQuestItemController questOption;
    public UIRecruitCharacterController recruitCharacter;
    private InnEncounterNode node;

    public void Show(InnEncounterNode node, Party party)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = party;
        this.inn = node.inn;
    Debug.Log(party);
       
        // for (int i=0; i<inn.shopItems.Count; i++)
        // {
        //     bool affordable = party.money >= inn.shopItems[i].cost;
        //     shopItems[i].SetValues(inn.shopItems[i], affordable, this);
        // }
        //questOption.SetValues(inn.quest);
        //recruitCharacter.SetValues(inn.recruitableCharacter);
       // FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
    }

    public void ContinueClicked()
    {
        canvas.enabled=false;
        node.Continue();
        FindObjectOfType<UICharacterViewController>().Hide();
    }

    public void SpecialClicked()
    {
        inn.Special(party);
        ContinueClicked();
    }
    public void DrinkClicked()
    {
        inn.Drink(party);
        ContinueClicked();
    }
    public void FoodCLicked()
    {
        inn.Eat(party);
        ContinueClicked();
    }
    public void RestClicked()
    {
        inn.Rest(party);
        ContinueClicked();
    }

    public void AcceptQuestClicked()
    {
        
    }

    public void RecruitCharacterClicked()
    {
        
    }

    public void UpdateUI()
    {
        //
    }
}
