using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
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
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    public Transform itemParent;
    public GameObject shopItemPrefab;
    private Merchant merchant;
    public BuyItemUI buyItemUI;
    private ShopItem selectedItem;
    private List<GameObject> instantiatedItems= new List<GameObject>();
    public Button switchBuyButton;
    public Button switchSellButton;
 
    public TextMeshProUGUI InStoreLabel;

    public void Show(MerchantEncounterNode node, Party party)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = party;
        this.merchant = node.merchant;
        shopItems = new List<UIShopItemController>();
        selectedItem = merchant.shopItems[0];
        buying = true;
        UpdateUI();
        //GameObject.FindObjectOfType<UIConvoyController>().Show();
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

        if (buying)
        {
            Debug.Log("Buying");
       
            InStoreLabel.text = "In Store:";
           
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
            if(selectedItem!=null)
                buyItemUI.Show(selectedItem.item,  party.money >= merchant.shopItems[0].cost, buying);
            else
            {
                buyItemUI.Hide();
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
                bool affordable =true; //Because we are selling
    
                shopItems[i].SetValues(new ShopItem(item.item, item.stock), affordable, this);
            }
            if(selectedItem !=null)
                buyItemUI.Show(selectedItem.item,  true, buying);
            else
            {
                buyItemUI.Hide();
            }
    
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

        buyItemUI.Hide();
        
        UpdateUI();
    }
    public void ItemClicked(ShopItem item)
    {
        selectedItem = item;
        Debug.Log(item.name+ " "+item.cost);
        if(buying)
            buyItemUI.Show(item.item,  party.money >= item.cost, buying);
        else
            buyItemUI.Show(item.item,  true, buying);
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
        if(merchant.shopItems.Count!=0)
            selectedItem = merchant.shopItems[0];
        UpdateUI();
    }
    public void SwitchSellClicked()
    {
        buying = false;
        if (party.Convoy.Items.Count != 0)
            selectedItem = new ShopItem(party.Convoy.Items[0].item, party.Convoy.Items[0].stock);
        UpdateUI();
    }
}
