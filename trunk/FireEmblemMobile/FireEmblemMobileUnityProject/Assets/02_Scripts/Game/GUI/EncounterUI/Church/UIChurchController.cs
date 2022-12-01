using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChurchController : MonoBehaviour, IShopItemClickedReceiver
{
    public enum ChurchUIState
    {
        Store,Pray,Blessing
    }
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
    [SerializeField] BuyItemUI buyItemUI;
    [SerializeField] UIBlessingArea blessingUI;
    [SerializeField] UIPrayArea prayUI;
    [SerializeField] private GameObject prayButton;
    [SerializeField] private GameObject saleButton;
    [SerializeField] private TextMeshProUGUI inStoreText;
    private ShopItem selectedItem;
    private List<GameObject> instantiatedItems= new List<GameObject>();
    private ChurchUIState state = ChurchUIState.Store;
    private BlessingBP blessingBp;
 
    public void UpdateUI()
    {
        if (state == ChurchUIState.Pray && !church.CanDonate(party.ActiveUnit))
            state = ChurchUIState.Blessing;
        if (state == ChurchUIState.Blessing && church.CanDonate(party.ActiveUnit))
            state = ChurchUIState.Pray;
        shopItems.Clear();
        saleButton.gameObject.SetActive(false);
        prayButton.gameObject.SetActive(false);
        prayUI.Hide();
        blessingUI.Hide();
        buyItemUI.Hide();
        for (int i = instantiatedItems.Count - 1; i >= 0; i--)
        {
            Destroy(instantiatedItems[i]);
        }
        instantiatedItems.Clear();
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        if (state == ChurchUIState.Store)
        {
            for (int i = 0; i < church.shopItems.Count; i++)
            {
                var go = Instantiate(shopItemPrefab, itemParent);
                var item = church.shopItems[i];
                instantiatedItems.Add(go);
                shopItems.Add(go.GetComponent<UIShopItemController>());
                bool affordable = party.CanAfford(item.cost);

                shopItems[i].SetValues(item, affordable, this);
            }

            if (church.shopItems.Count >= 1)
                buyItemUI.Show(church.shopItems[0].Item, party.CanAfford(church.shopItems[0].cost), true);
            else
            {
                buyItemUI.Hide();
            }
            saleButton.gameObject.SetActive(false);
            prayButton.gameObject.SetActive(true);
            inStoreText.gameObject.SetActive(true);
        }
        else if (state == ChurchUIState.Pray)
        {
            saleButton.gameObject.SetActive(true);
            prayButton.gameObject.SetActive(false);
            inStoreText.gameObject.SetActive(false);
            prayUI.Show(party.ActiveUnit);
        }
        else if (state == ChurchUIState.Blessing)
        {
            saleButton.gameObject.SetActive(true);
            prayButton.gameObject.SetActive(false);
            inStoreText.gameObject.SetActive(false);
            blessingUI.Show(party.ActiveUnit,blessingBp, false);
        }
        UpdateSelectionColors();
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
        party.Convoy.AddItem(selectedItem.Item);
        church.RemoveItem(selectedItem.Item);
        SelectNextItem();
        buyItemUI.Hide();
        UpdateUI();
    }
    private void SelectNextItem()
    {
        
        if(church.shopItems.Count!=0)
            selectedItem = church.shopItems[0];
        else
        {
            selectedItem = null;
        }
        
     
    }
    public void ItemClicked(ShopItem item)
    {
        selectedItem = item;
        UpdateUI();
        buyItemUI.Show(item.Item,  party.CanAfford(item.cost), true);
    }

    public void Hide()
    {
        canvas.enabled = false;
    }
    public void Show(ChurchEncounterNode node, Party party)
    {
        blessingBp = null;
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.church = node.church;
        shopItems = new List<UIShopItemController>();
        selectedItem = church.shopItems[0];
        UpdateUI();
        //FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
    }

    
    public void PrayClicked()
    {
        state = ChurchUIState.Pray;
        if (!church.CanDonate(party.ActiveUnit))
            state = ChurchUIState.Blessing;
        UpdateUI();
    }
    public void SaleClicked()
    {
        state = ChurchUIState.Store;
        UpdateUI();
    }
    public void ContinueClicked()
    {
        canvas.enabled = false;
        FindObjectOfType<UICharacterViewController>().Hide();
        node.Continue();
    }
    void UpdateSelectionColors()
    {
        Debug.Log("Update Selection Colors");
        foreach (var shopItem in shopItems)
        {
            shopItem.Deselect();
            Debug.Log("Deselect Item: "+shopItem.item.name);
            if (shopItem.item.Equals(selectedItem))
            {
                Debug.Log("Select Item: "+shopItem.item.name);
                shopItem.Select();
            }
        }
    }
    
    public void DonateSmall()
    {
        blessingBp=church.DonateSmall(party.ActiveUnit, party.ActiveUnit.Stats.Attributes.FAITH);
        state = ChurchUIState.Blessing;
        UpdateUI();
        
    }

    public void DonateMedium()
    {
        blessingBp=church.DonateMedium(party.ActiveUnit,party.ActiveUnit.Stats.Attributes.FAITH);
        state = ChurchUIState.Blessing;
        UpdateUI();
    }

    public void DonateHigh()
    {
        blessingBp=church.DonateHigh(party.ActiveUnit,party.ActiveUnit.Stats.Attributes.FAITH);
        state = ChurchUIState.Blessing;
        UpdateUI();
    }

    public void AcceptBlessing()
    {
        Debug.Log("Accept Blessing");
    }
}
