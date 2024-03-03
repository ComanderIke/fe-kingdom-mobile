using System;
using System.Collections.Generic;
using Game.EncounterAreas.Model;
using Game.GameActors.Player;
using Game.GUI.CharacterScreen;
using Game.GUI.Convoy;
using Game.GUI.EncounterUI.Smithy;
using Game.GUI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.EncounterUI.Merchant
{
    public class UIMerchantController : MonoBehaviour,IShopItemClickedReceiver
    {
    
        public Canvas canvas;
        [HideInInspector]
        public Party party;
        // public TextMeshProUGUI personName;
        //public TextMeshProUGUI talkText;
        private List<UIConvoyItemController> shopItems;
        [SerializeField] private UICharacterFace characterFace;
        [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
        public Transform itemParent;
        public GameObject shopItemPrefab;
        private EncounterAreas.Encounters.Merchant.Merchant merchant;
        public BuyItemUI buyItemUI;
        private int selectedItemIndex=0;
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

        public void Show(EncounterAreas.Encounters.Merchant.Merchant merchant, Party party)
        {
            canvas.enabled = true;
            this.party = party;
            this.merchant = merchant;
            shopItems = new List<UIConvoyItemController>();
            selectedItemIndex= 0;
            buying = true;
            npcFaceController.Show("Travelers are welcome to check out these wares.");
            party.onActiveUnitChanged -= ActiveUnitChanged;
            party.onActiveUnitChanged += ActiveUnitChanged;
            UpdateUI(true);
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
        public void UpdateUI(bool first=false)
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
                        shopItems.Add(go.GetComponent<UIConvoyItemController>());
                        bool affordable = party.CanAfford(merchant.GetCost(merchant.shopItems[i].item));

                        shopItems[i].SetValues(item, affordable, first,this, true);
                    
                    }
                    if(selectedItemIndex< shopItems.Count)
                        buyItemUI.Show( shopItems[selectedItemIndex].stockedItem.item, merchant.GetSellCost(shopItems[selectedItemIndex].stockedItem.item),party.CanAfford(merchant.GetCost(merchant.shopItems[0].item)), buying);
               
                }

            }
            else
            {
            
                if (party.Convoy.Items.Count == 0)
                {
                    SoldOutArea.SetStateNothingToSell();
                    buyItemUI.Hide();
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
                    shopItems.Add(go.GetComponent<UIConvoyItemController>());
                    bool affordable =true; //Because we are selling
    
                    shopItems[i].SetValues(item, affordable, first,this, true);
                }
                if(selectedItemIndex< shopItems.Count)
                    buyItemUI.Show( shopItems[selectedItemIndex].stockedItem.item,  merchant.GetCost(shopItems[selectedItemIndex].stockedItem.item),true, buying);
    
                switchBuyButton.interactable = true;
                switchSellButton.interactable = false;
       
            }
            UpdateSelectionColors();
        
        }

        public void BuyClicked()
        {
            if (buying)
            {
                merchant.Buy( shopItems[selectedItemIndex].stockedItem.item);
            
            }
            else
            {
                merchant.Sell( shopItems[selectedItemIndex].stockedItem.item);
          
            }

            SelectNextItem();
            //buyItemUI.Hide();
        
            UpdateUI();
        }

        private void SelectNextItem()
        {
            selectedItemIndex = 0;
        }

        void UpdateSelectionColors()
        {

            foreach (var shopItem in shopItems)
            {
                shopItem.Deselect();
  
                if (shopItem.stockedItem.Equals( shopItems[selectedItemIndex].stockedItem))
                {
           
                    shopItem.Select();
                }
            }
        }
        public void ItemClicked(UIConvoyItemController itemController)
        {
            shopItems[selectedItemIndex].Deselect();

            selectedItemIndex = shopItems.IndexOf(itemController);
            shopItems[selectedItemIndex].Select();
            UpdateUI();
            //Debug.Log(item.name+ " "+item.cost);
            if(buying)
                buyItemUI.Show( shopItems[selectedItemIndex].stockedItem.item,  merchant.GetCost( shopItems[selectedItemIndex].stockedItem.item),party.CanAfford(merchant.GetCost( shopItems[selectedItemIndex].stockedItem.item)), buying);
            else
                buyItemUI.Show( shopItems[selectedItemIndex].stockedItem.item,  merchant.GetSellCost(shopItems[selectedItemIndex].stockedItem.item),true, buying);
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
            selectedItemIndex = 0;
            UpdateUI();
        }
        public void SwitchSellClicked()
        {
            buying = false;
            selectedItemIndex = 0;
            UpdateUI();
        }
    }
}
