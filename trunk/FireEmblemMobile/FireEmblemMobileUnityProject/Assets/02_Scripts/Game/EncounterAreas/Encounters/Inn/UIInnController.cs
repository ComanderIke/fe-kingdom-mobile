using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
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
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    [SerializeField] private TextMeshProUGUI restDescription;
    [SerializeField] private TextMeshProUGUI drinkDescription;
    [SerializeField] private TextMeshProUGUI eatDescription;
    [SerializeField] private TextMeshProUGUI restPriceText;
    [SerializeField] private TextMeshProUGUI drinkPriceText;
    [SerializeField] private TextMeshProUGUI eatPriceText;
    [SerializeField] private GameObject restCoinIcon;
    [SerializeField] private GameObject drinkCoinIcon;
    [SerializeField] private GameObject eatCoinIcon;
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
    UpdateUI();
    // for (int i=0; i<inn.shopItems.Count; i++)
    // {
    //     bool affordable = party.money >= inn.shopItems[i].cost;
    //     shopItems[i].SetValues(inn.shopItems[i], affordable, this);
    // }
    //questOption.SetValues(inn.quest);
    //recruitCharacter.SetValues(inn.recruitableCharacter);
    // FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
    }
    public void NextClicked()
    {
        Player.Instance.Party.ActiveUnitIndex++;
        UpdateUI();
    }

    public void PrevClicked()
    {
        Player.Instance.Party.ActiveUnitIndex--;
        UpdateUI();
    }
    public void Hide()
    {
        canvas.enabled = false;
    }


    public void SpecialClicked()
    {
        inn.Special(Player.Instance.Party.ActiveUnit);
        UpdateUI();
    }
    public void DrinkClicked()
    {
        inn.Drink( Player.Instance.Party.ActiveUnit);
        UpdateUI();
    }
    public void FoodCLicked()
    {
        inn.Eat(Player.Instance.Party.ActiveUnit);
        UpdateUI();
    }
    public void RestClicked()
    {
        inn.Rest(Player.Instance.Party.ActiveUnit);
        UpdateUI();
    }
    public void ContinueClicked()
    {
        canvas.enabled=false;
        node.Continue();
        FindObjectOfType<UICharacterViewController>().Hide();
    }
    public void AcceptQuestClicked()
    {
        
    }

    public void RecruitCharacterClicked()
    {
        
    }

    public void UpdateUI()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        

        restPriceText.text = GetCostText(inn.GetRestPrice(), restCoinIcon);
        drinkPriceText.text = GetCostText(inn.GetDrinkPrice(), drinkCoinIcon);
        eatPriceText.text = GetCostText(inn.GetEatPrice(), eatCoinIcon);

        restDescription.text = GetDescriptionText(inn.GetRestHeal());

        drinkDescription.text = GetDescriptionText(inn.GetDrinkHeal());
        eatDescription.text = GetDescriptionText(inn.GetEatHeal());
    }
    private string GetDescriptionText(int heal)
    {
        return "Heal " + heal + " % Hp";
    }
    private string GetCostText(int cost, GameObject coinIcon)
    {
        
        string costText = ""+cost;
        coinIcon.gameObject.SetActive(cost != 0);
        if (cost == 0)
        {
            costText = "Free";
            
        }

        return costText;
    }
}
