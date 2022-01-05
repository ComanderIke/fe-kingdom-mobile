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


    public void Show(Party party, Inn inn)
    {
        canvas.enabled = true;
        this.party = party;

       
        for (int i=0; i<inn.shopItems.Count; i++)
        {
            shopItems[i].SetValues(inn.shopItems[i]);
        }
        questOption.SetValues(inn.quest);
        recruitCharacter.SetValues(inn.recruitableCharacter);
    }

    public void Hide()
    {
        canvas.enabled=false;
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
