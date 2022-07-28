using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMerchantController : MonoBehaviour,IShopItemClickedReceiver
{
    private MerchantEncounterNode node;
    public Canvas canvas;
    [HideInInspector]
    public Party party;
    // public TextMeshProUGUI personName;
    //public TextMeshProUGUI talkText;
    private List<UIShopItemController> shopItems;
    public Transform itemParent;
    public GameObject shopItemPrefab;
    private Merchant merchant;
    public SelectedItemUI selectedItemUI;
    private ShopItem selectedItem;
    private List<GameObject> instantiatedItems= new List<GameObject>();
    public Button switchBuyButton;
    public Button switchSellButton;
    public TextMeshProUGUI BuySellButtonText;
    public TextMeshProUGUI InStoreLabel;
    public TextMeshProUGUI MerchantDialog;
    public void Show(MerchantEncounterNode node, Party party)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = party;
        this.merchant = node.merchant;
        shopItems = new List<UIShopItemController>();
        UpdateUI();
        //GameObject.FindObjectOfType<UIConvoyController>().Show();
    }

    public void UpdateUI()
    { 
        shopItems.Clear();
        for (int i = instantiatedItems.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedItems[i]);
        }
        instantiatedItems.Clear();
        

        if (buying)
        {
            Debug.Log("Buying");
            BuySellButtonText.text = "Buy";
            InStoreLabel.text = "In Store:";
            MerchantDialog.text = "What are you buying?";
            switchBuyButton.interactable = false;
            switchSellButton.interactable = true;
            for (int i=0; i<merchant.shopItems.Count; i++)
            {
                var go=Instantiate(shopItemPrefab, itemParent);
                var item = merchant.shopItems[i];
                instantiatedItems.Add(go);
                shopItems.Add(go.GetComponent<UIShopItemController>());
                bool affordable = party.money >= item.cost;
    
                shopItems[i].SetValues(item, affordable, this);
            }
            if(merchant.shopItems.Count>=1)
                selectedItemUI.Show(merchant.shopItems[0],  party.money >= merchant.shopItems[0].cost);
            else
            {
                selectedItemUI.Hide();
            }
        }
        else
        {
            Debug.Log("Selling");
            for (int i=0; i<party.Convoy.Items.Count; i++)
            {
                var go=Instantiate(shopItemPrefab, itemParent);
                var item = party.Convoy.Items[i];
                instantiatedItems.Add(go);
                shopItems.Add(go.GetComponent<UIShopItemController>());
                bool affordable = party.money >= item.item.cost;
    
                shopItems[i].SetValues(new ShopItem(item.item, item.stock), affordable, this);
            }
            if(party.Convoy.Items.Count>=1)
                selectedItemUI.Show(new ShopItem(party.Convoy.Items[0].item, party.Convoy.Items[0].stock),  true);
            else
            {
                selectedItemUI.Hide();
            }
            BuySellButtonText.text = "Sell";
            MerchantDialog.text = "What are you selling?";
            switchBuyButton.interactable = true;
            switchSellButton.interactable = false;
            InStoreLabel.text = "In Convoy:";
        }

    }

    public void BuyClicked()
    {
        if (buying)
        {
            party.Money -= selectedItem.cost;
            party.Convoy.AddItem(selectedItem.item);
            merchant.RemoveItem(selectedItem.item);
        }
        else
        {
            party.Money += selectedItem.cost;
            party.Convoy.RemoveItem(selectedItem.item);
        }

        selectedItemUI.Hide();
        
        UpdateUI();
    }
    public void ItemClicked(ShopItem item)
    {
        selectedItem = item;
        Debug.Log(item.name+ " "+item.cost);
        if(buying)
            selectedItemUI.Show(item,  party.money >= item.cost);
        else
            selectedItemUI.Show(item,  true);
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
    public void ContinueClicked()
    {
        canvas.enabled=false;
        node.Continue();
        FindObjectOfType<UICharacterViewController>().Hide();
    }

    public bool buying = true;
    public void SwitchBuyClicked()
    {
        buying = true;
        UpdateUI();
    }
    public void SwitchSellClicked()
    {
        buying = false;
        UpdateUI();
    }
}
