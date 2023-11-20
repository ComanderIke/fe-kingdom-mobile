using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMerchantController : MonoBehaviour,IShopItemClickedReceiver
{
    
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
    [SerializeField] private NPCFaceController npcFaceController;
    private List<GameObject> instantiatedItems= new List<GameObject>();
    public Button switchBuyButton;
    public Button switchSellButton;
    [SerializeField] private TextMeshProUGUI merchantNameText;
    [SerializeField] private Image merchantFaceImage;
    [SerializeField] private UISoldOutArea SoldOutArea;
   

    public static event Action OnFinished;
 
    private void OnDestroy()
    {
        party.onActiveUnitChanged -= ActiveUnitChanged;
    }

    public void Show(Merchant merchant, Party party)
    {
        canvas.enabled = true;
        this.party = party;
        this.merchant = merchant;
        shopItems = new List<UIShopItemController>();
        selectedItem = merchant.shopItems[0];
        buying = true;
        npcFaceController.Show("Travelers are welcome to check out these wares.");
        party.onActiveUnitChanged -= ActiveUnitChanged;
        party.onActiveUnitChanged += ActiveUnitChanged;
        UpdateUI();
        //GameObject.FindObjectOfType<UIConvoyController>().Show();
    }
    void ActiveUnitChanged()
    {
        UpdateUI();
        
    }
    public void NextClicked()
    {
        Player.Instance.Party.ActiveUnitIndex++;
        
    }

    public void PrevClicked()
    {
        Player.Instance.Party.ActiveUnitIndex--;
        
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
        merchantFaceImage.sprite = merchant.merchantFace;
        merchantNameText.SetText(merchant.merchantName);
      
        if (buying)
        {
            
            
           
            switchBuyButton.interactable = false;
            switchSellButton.interactable = true;
            if (merchant.shopItems.Count == 0)
            {
                
                SoldOutArea.SetStateSoldOut();

                buyItemUI.Hide();
            }
            else
            {

                SoldOutArea.Hide();

                for (int i = 0; i < merchant.shopItems.Count; i++)
                {
                    var go = Instantiate(shopItemPrefab, itemParent);
                    var item = merchant.shopItems[i];
                    instantiatedItems.Add(go);
                    shopItems.Add(go.GetComponent<UIShopItemController>());
                    bool affordable = party.CanAfford(merchant.GetCost(merchant.shopItems[i]));

                    shopItems[i].SetValues(item, affordable, this);
                }

                if (selectedItem != null)
                    buyItemUI.Show(selectedItem.Item, party.CanAfford(merchant.GetCost(merchant.shopItems[0])), buying);
                else
                {
                    buyItemUI.Hide();
                }
            }

        }
        else
        {
            if (party.Convoy.Items.Count == 0)
            {
                SoldOutArea.SetStateNothingToSell();
            }
            else
            {
                SoldOutArea.Hide();
            }

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
                buyItemUI.Show(selectedItem.Item,  true, buying);
            else
            {
                buyItemUI.Hide();
            }
    
            switchBuyButton.interactable = true;
            switchSellButton.interactable = false;
       
        }
        UpdateSelectionColors();
        
    }

    public void BuyClicked()
    {
        if (buying)
        {
            merchant.Buy(selectedItem);
            
        }
        else
        {
            merchant.Sell(selectedItem);
          
        }

        SelectNextItem();
        //buyItemUI.Hide();
        
        UpdateUI();
    }

    private void SelectNextItem()
    {
        if (buying)
        {
            if(merchant.shopItems.Count!=0)
                selectedItem = merchant.shopItems[0];
            else
            {
                selectedItem = null;
            }
        }
        else
        {
            if (party.Convoy.Items.Count != 0)
                selectedItem = new ShopItem(party.Convoy.Items[0].item, party.Convoy.Items[0].stock);
            else
            {
                selectedItem = null;
            }
        }
    }

    void UpdateSelectionColors()
    {

        foreach (var shopItem in shopItems)
        {
            shopItem.Deselect();
  
            if (shopItem.item.Equals(selectedItem))
            {
           
                shopItem.Select();
            }
        }
    }
    public void ItemClicked(ShopItem item)
    {
        selectedItem = item;
        UpdateUI();
        Debug.Log(item.name+ " "+item.cost);
        if(buying)
            buyItemUI.Show(item.Item,  party.CanAfford(merchant.GetCost(item)), buying);
        else
            buyItemUI.Show(item.Item,  true, buying);
    }

    public void Hide()
    {
        canvas.enabled = false;
        party.onActiveUnitChanged -= ActiveUnitChanged;
     
    }
    public void ContinueClicked()
    {
        canvas.enabled=false;
        OnFinished?.Invoke();
        FindObjectOfType<UICharacterViewController>().Hide();
    }

    public bool buying = true;
    public void SwitchBuyClicked()
    {
        buying = true;
        if (merchant.shopItems.Count != 0)
            selectedItem = merchant.shopItems[0];
        else
        {
            selectedItem = null;
        }
        UpdateUI();
    }
    public void SwitchSellClicked()
    {
        buying = false;
        if (party.Convoy.Items.Count != 0)
            selectedItem = new ShopItem(party.Convoy.Items[0].item, party.Convoy.Items[0].stock);
        else
        {
            selectedItem = null;
            
        }
        UpdateUI();
    }
}
