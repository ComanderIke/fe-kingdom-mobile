using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIChurchController : MonoBehaviour, IShopItemClickedReceiver
{
    public Canvas canvas;
    public ChurchEncounterNode node;
    [HideInInspector]
    public Party party;
    private List<UIShopItemController> shopItems;
    public Transform itemParent;
    public GameObject shopItemPrefab;
    private Church church;    
    public SelectedItemUI selectedItemUI;
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
            selectedItemUI.Show(church.shopItems[0],  party.money >= church.shopItems[0].cost);
        else
        {
            selectedItemUI.Hide();
        }
    }

    public void BuyClicked()
    {
        party.Money -= selectedItem.cost;
        party.Convoy.AddItem(selectedItem.item);
        church.RemoveItem(selectedItem.item);
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
