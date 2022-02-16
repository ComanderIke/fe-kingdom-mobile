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
    public void Show(MerchantEncounterNode node, Party party)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = party;
        this.merchant = node.merchant;
        shopItems = new List<UIShopItemController>();
        for (int i=0; i<merchant.shopItems.Count; i++)
        {
            var go=Instantiate(shopItemPrefab, itemParent);
            var item = merchant.shopItems[i];
            shopItems.Add(go.GetComponent<UIShopItemController>());
            bool affordable = party.money >= item.cost;
            Debug.Log("Merchant Item: "+item.Name);
            shopItems[i].SetValues(new ShopItem(item.Name, item.cost, item.Sprite, item.Description), affordable);
        }
        //GameObject.FindObjectOfType<UIConvoyController>().Show();
    }

    public void ItemClicked(ShopItem item)
    {
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
