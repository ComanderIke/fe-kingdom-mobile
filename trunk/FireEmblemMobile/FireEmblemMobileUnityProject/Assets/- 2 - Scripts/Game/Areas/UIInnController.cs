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
       
        for (int i=0; i<inn.shopItems.Count; i++)
        {
            bool affordable = party.money >= inn.shopItems[i].cost;
            shopItems[i].SetValues(inn.shopItems[i], affordable);
        }
        questOption.SetValues(inn.quest);
        recruitCharacter.SetValues(inn.recruitableCharacter);
        FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
    }

    public void ContinueClicked()
    {
        canvas.enabled=false;
        node.Continue();
        FindObjectOfType<UICharacterViewController>().Hide();
    }

    public void DrinkClicked()
    {
        
    }
    public void FoodCLicked()
    {
        
    }
    public void RestClicked()
    {
        
    }

    public void AcceptQuestClicked()
    {
        
    }

    public void RecruitCharacterClicked()
    {
        
    }
}
