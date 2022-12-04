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
    private InnEncounterNode node;

    public void Show(InnEncounterNode node, Party party)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = party;
        this.inn = node.inn;
    Debug.Log(party);
    foreach (var shopItem in shopItems)
    {
        shopItem.Select();
    }
    party.onActiveUnitChanged -= ActiveUnitChanged;
    party.onActiveUnitChanged += ActiveUnitChanged;
    
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
    }

    public void PrevClicked()
    {
        Player.Instance.Party.ActiveUnitIndex--;
    }
    public void Hide()
    {
        canvas.enabled = false;
        party.onActiveUnitChanged -= ActiveUnitChanged;
   
    }

    void ActiveUnitChanged()
    {
        UpdateUI();
        
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

        shopItems[0].SetAffordable(Player.Instance.Party.CanAfford(inn.GetRestPrice()));
        shopItems[1].SetAffordable(Player.Instance.Party.CanAfford(inn.GetDrinkPrice()));
        shopItems[2].SetAffordable(Player.Instance.Party.CanAfford(inn.GetEatPrice()));
        restDescription.text = GetDescriptionText(inn.GetRestHeal());

        drinkDescription.text = GetDescriptionText(inn.GetDrinkHeal());
        eatDescription.text = GetDescriptionText(inn.GetEatHeal());
        if (!inn.CanUnitRest(party.ActiveUnit))
        {
            shopItems[0].SetAffordable(false);
            restDescription.text = "Already rested.";
        }
        if (!inn.CanUnitDrink(party.ActiveUnit))
        {
            shopItems[1].SetAffordable(false);
            drinkDescription.text = "Already drank.";
        }
        if (!inn.CanUnitEat(party.ActiveUnit))
        {
            shopItems[2].SetAffordable(false);
            eatDescription.text = "Already ate.";
        }

        restPriceText.text = GetCostText(inn.GetRestPrice(), restCoinIcon);
        drinkPriceText.text = GetCostText(inn.GetDrinkPrice(), drinkCoinIcon);
        eatPriceText.text = GetCostText(inn.GetEatPrice(), eatCoinIcon);

       
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
