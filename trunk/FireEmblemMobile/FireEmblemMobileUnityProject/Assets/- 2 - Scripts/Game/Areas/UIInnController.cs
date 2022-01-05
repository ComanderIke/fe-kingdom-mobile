using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEditor.Experimental.GraphView;
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

    public void Show(InnEncounterNode node)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = node.party;
        this.inn = node.inn;

       
        for (int i=0; i<inn.shopItems.Count; i++)
        {
            shopItems[i].SetValues(inn.shopItems[i]);
        }
        questOption.SetValues(inn.quest);
        recruitCharacter.SetValues(inn.recruitableCharacter);
    }

    public void ContinueClicked()
    {
        canvas.enabled=false;
        node.Continue();
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
