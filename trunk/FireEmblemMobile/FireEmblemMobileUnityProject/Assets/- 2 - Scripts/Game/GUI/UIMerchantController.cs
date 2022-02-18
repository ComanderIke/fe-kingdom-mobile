using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIMerchantController : MonoBehaviour
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

    private void UpdateUI()
    { 
        shopItems.Clear();
        for (int i = instantiatedItems.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedItems[i]);
        }
        instantiatedItems.Clear();
        for (int i=0; i<merchant.shopItems.Count; i++)
        {
            var go=Instantiate(shopItemPrefab, itemParent);
            var item = merchant.shopItems[i];
            instantiatedItems.Add(go);
            shopItems.Add(go.GetComponent<UIShopItemController>());
            bool affordable = party.money >= item.cost;
            Debug.Log("Merchant Item: "+item.name);
            shopItems[i].SetValues(item, affordable);
        }
        
    }

    public void BuyClicked()
    {
        party.Money -= selectedItem.cost;
        party.AddItem(selectedItem.item);
        merchant.RemoveItem(selectedItem.item);
        selectedItemUI.Hide();
        UpdateUI();
    }
    public void ItemClicked(ShopItem item)
    {
        selectedItem = item;
        selectedItemUI.Show(item,  party.money >= item.cost);
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
}
