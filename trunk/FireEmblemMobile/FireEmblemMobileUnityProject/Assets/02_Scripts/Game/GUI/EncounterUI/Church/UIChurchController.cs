using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIChurchController : MonoBehaviour, IShopItemClickedReceiver
{
    public Canvas canvas;
    public ChurchEncounterNode node;
    [HideInInspector]
    public Party party;
    private List<UIShopItemController> shopItems;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    public Transform itemParent;
    public GameObject shopItemPrefab;
    private Church church;    
    public BuyItemUI buyItemUI;
    private ShopItem selectedItem;
    private List<GameObject> instantiatedItems= new List<GameObject>();
 
    public void UpdateUI()
    { 
        shopItems.Clear();
        for (int i = instantiatedItems.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedItems[i]);
        }
        instantiatedItems.Clear();
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        for (int i=0; i<church.shopItems.Count; i++)
        {
            var go=Instantiate(shopItemPrefab, itemParent);
            var item = church.shopItems[i];
            instantiatedItems.Add(go);
            shopItems.Add(go.GetComponent<UIShopItemController>());
            bool affordable = party.money >= item.cost;
    
            shopItems[i].SetValues(item, affordable, this);
        }
        if(church.shopItems.Count>=1)
            buyItemUI.Show(church.shopItems[0].item,  party.money >= church.shopItems[0].cost, true);
        else
        {
            buyItemUI.Hide();
        }
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
    public void BuyClicked()
    {
        party.Money -= selectedItem.cost;
        party.Convoy.AddItem(selectedItem.item);
        church.RemoveItem(selectedItem.item);
        buyItemUI.Hide();
        UpdateUI();
    }
    public void ItemClicked(ShopItem item)
    {
        selectedItem = item;
        buyItemUI.Show(item.item,  party.money >= item.cost, true);
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
    public void Show(ChurchEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.church = node.church;
        shopItems = new List<UIShopItemController>();
        UpdateUI();
        //FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
    }

    public void PrayClicked()
    {
        
    }
    public void ContinueClicked()
    {
        canvas.enabled = false;
        FindObjectOfType<UICharacterViewController>().Hide();
        node.Continue();
    }
}
