using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using __2___Scripts.Game.Utility;
using Game.GameActors.Items;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Utility;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UIConvoyController:MonoBehaviour
{
    public enum ConvoyContext
    {
        Default,
        SelectRelic,
        ChooseGem,
        Battle,
        SelectCombatItem
    }
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject noneButton;
    [SerializeField] private UIEquipmentController equipmentController;
    [SerializeField] private UICharacterViewController charView;
    [SerializeField] private ClickAndHoldButton equipButton;
    [SerializeField] private ClickAndHoldButton useButton;
    [SerializeField] private ClickAndHoldButton dropButton;
    [SerializeField] private TextMeshProUGUI contextText;
    [SerializeField] private Color EquipColor;
    [SerializeField] private Color UseColor;

    public GameObject convoyItemPrefab;
    public List<ConvoyDropArea> DropAreas;

    private List<GameObject> instantiatedItems;
    private Convoy convoy;
    private bool init = false;
    private Type typeFilter;
    private bool itemClicked = false;
    private ConvoyContext context;
    private float timeShown = 0;
    private void Start()
    {
     
        useButton.OnClick -= UseClicked;
        dropButton.OnClick -= DropButtonClicked;
        equipButton.OnClick += EquipClicked;
        dropButton.OnClick += DropButtonClicked;
    }
    private void Update()
    {
        if(!canvas.enabled)
          return;
        timeShown += Time.deltaTime;
        if (InputUtility.TouchEnd()&&convoy != null&& timeShown>=.3f)
        {
            if (!itemClicked&& !useButton.WasPressingUntilLastFrame&&!equipButton.WasPressingUntilLastFrame&&!dropButton.WasPressingUntilLastFrame)
            {
                // Debug.Log("Deselect");
                convoy.Deselect();
          
                UpdateValues();
                // if(!UIHelper.IsPointerOverUIObject())
                //     Hide();
            }
            else
            {
                // Debug.Log("Dont Deselect");
            }
            //
        }

        itemClicked = false;
    }
    public void Toogle()
    {
        if(canvas.enabled)
            Hide();
        else
        {
            Show();
        }
    }
    void DropButtonClicked()
    {
        Debug.Log("DropClicked");
        var dropItem = convoy.SelectedItem;
        convoy.Deselect();
        convoy.RemoveStockedItem(dropItem);
    }

    public void UseClicked()
    {
        var contextItem = convoy.SelectedItem;
        context = ConvoyContext.Default;
        if(contextItem==null)
            return;
        if (contextItem.item is ConsumableItem consumableItem)
        {
            convoy.Deselect();
            consumableItem.Use(Player.Instance.Party.ActiveUnit, Player.Instance.Party);
        }

       
        charView.UpdateUnit(Player.Instance.Party.ActiveUnit);
        
    }

    void EquipClicked()
    {
        Debug.Log("ContextClicked: "+context);
        var contextItem = convoy.SelectedItem;
        context = ConvoyContext.Default;
        if(contextItem==null)
            return;
        if (contextItem.item is Relic relic)
        {
            convoy.Deselect();
            Player.Instance.Party.ActiveUnit.Equip(relic);
         
            
        }
        else if (contextItem.item is IEquipableCombatItem equipableItem)
        {
                var selectedCombatItem = new StockedCombatItem(equipableItem, contextItem.stock);
                convoy.Deselect();
                Player.Instance.Party.ActiveUnit.Equip(selectedCombatItem,equipmentController.selectedSlotCombatItemSlot.slotNumber);
        }
        charView.UpdateUnit(Player.Instance.Party.ActiveUnit);
        
    }
  
    public void Show()
    {
        MyDebug.LogUI("Show Convoy");
        Show(typeof(Item), ConvoyContext.Default);
    }
    public void Show(ConvoyContext context)
    {
        Show(typeof(Item), context);
    }
    public void Show(Type filter, ConvoyContext context)
    {
        timeShown = 0;
        this.convoy = Player.Instance.Party.Convoy;
        this.context = context;
        typeFilter = filter;
       
        canvas.enabled = true;
        if(!charView.IsVisible())
            charView.Show(Player.Instance.Party.ActiveUnit);

        if (typeFilter == typeof(Gem))
        {
            context = ConvoyContext.ChooseGem;
        }
        UpdateValues();
        //Debug.Log("Showing convoy! itemcount: " + convoy.Items.Count);
    }

    private UIConvoyItemController CreateItemGameObject(StockedItem stockedItem, int index)
    {
        if(typeFilter==null)
           typeFilter = typeof(Item);
        var go = Instantiate(convoyItemPrefab, DropAreas[index].transform);
        var itemController = go.GetComponent<UIConvoyItemController>();
        bool isNewItem = newItems.Contains(stockedItem);
        MyDebug.LogLogic("Create Item: "+stockedItem.item+" isNew: "+isNewItem);
      
      
       // Debug.Log("Item type: "+stockedItem.item.GetType()+" "+typeFilter.IsAssignableFrom(stockedItem.item.GetType()));
        itemController.SetValues(stockedItem,  index,isNewItem, !typeFilter.IsAssignableFrom(stockedItem.item.GetType()));
        if(isNewItem)
            newItems.Remove(stockedItem);
        itemController.onClicked += ItemClicked;
        // var dragController = go.GetComponent<UIDragable>();
        // dragController.SetItem(stockedItem.item);
        // dragController.SetCanvas(GetComponent<Canvas>());
        instantiatedItems.Add(go);
        return itemController;
    }

    void UpdateContext()
    {
        switch(context){
            case ConvoyContext.Default: contextText.text = "Convoy";
                break;
            case ConvoyContext.SelectRelic: contextText.text = "Select a relic to equip";
                break;
            case ConvoyContext.SelectCombatItem: contextText.text = "Select a combat item to equip";
                break;
        }
        var selectedItem =  convoy.SelectedItem;
        if (selectedItem != null)
        {
            Debug.Log("Selected  Item : "+selectedItem);
            dropButton.gameObject.SetActive(true);
            if (selectedItem.item is Relic)
            {
                equipButton.gameObject.SetActive(true);
               
            }

            if (context==ConvoyContext.SelectCombatItem)
            {
                if (selectedItem.item is IEquipableCombatItem)
                {
                    equipButton.gameObject.SetActive(true);
                    
                }
            }
            if (selectedItem.item is ConsumableItem)
            {
                useButton.gameObject.SetActive(true);

            }
        }
        else
        {
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            dropButton.gameObject.SetActive(false);
        }
    }

    private void UpdateValues()
    {
        UpdateContext();
        if (!init)
        {
            init = true;
            Player.Instance.Party.Convoy.convoyUpdated += UpdateConvoy;
            Player.Instance.Party.Convoy.itemAdded += AddedItem;
            instantiatedItems = new List<GameObject>();
            newItems = new List<StockedItem>();
        }

        if (instantiatedItems.Count != 0)
        {
            for (int i = instantiatedItems.Count - 1; i >= 0; i--)
            {
                Destroy(instantiatedItems[i]);
            }
            instantiatedItems.Clear();
        }
        
        var sortedList = new List<StockedItem>(convoy.Items);
        sortedList.Sort(delegate(StockedItem x, StockedItem y)
        {
           // Debug.Log("Compare: "+x.item.Name+" "+y.item.Name);
            if (x.item == null && y.item == null) return 0;
            if (x.item == null) return -1;
            if (y.item == null) return 1;
            if (x.item.GetType()== typeFilter)
            {
                // Debug.Log("X is Relic");
                
                if (y.item.GetType() == typeFilter)
                {
                    // Debug.Log("Both Relics");
                    return 0;
                }

                return -1;
            }

            if (y.item.GetType() == typeFilter)
            {
                // Debug.Log("Y is Relic");
                return 1;
            }
            // Debug.Log("No Gem");  
            return 0;
        });

        for (int i = 0; i < sortedList.Count; i++)
        {
            var itemController = CreateItemGameObject(sortedList[i], i);
            if (convoy.SelectedItem == sortedList[i])
            {
                DropAreas[i].Select();
                itemController.Select();
            }
            else
            {
                DropAreas[i].Deselect();
                itemController.Deselect();
            }
        }
        

        if (context != ConvoyContext.Default)
        {
            noneButton.gameObject.SetActive(true);
        }
        else
        {
            noneButton.gameObject.SetActive(false);
        }

        

    }
    private void UpdateConvoy()
    {
        Debug.Log("Update Convoy!");
        UpdateValues();
    }
    public void ShowGemOptions()
    {
        typeFilter = typeof(Gem);
        context = ConvoyContext.ChooseGem;
        UpdateValues();
    }

    private int selecteditemIndex = 0;
    private void ItemClicked(UIConvoyItemController clickedItem)
    {
        MyDebug.LogInput("ItemClicked "+clickedItem.stockedItem.item);
        itemClicked = true;
        convoy.Select(clickedItem.stockedItem);
        ToolTipSystem.Show(clickedItem.stockedItem, clickedItem.transform.position);
        UpdateValues();

    }
    private List<StockedItem> newItems;
    void AddedItem(StockedItem item)
    {
        // Debug.Log("Added item: "+item.item);
        newItems.Add(item);
        UpdateValues();
    }
    void DeselectAllItems()
    {
        foreach (var dropArea in DropAreas)
        {
            dropArea.Deselect();
        }
    }
    public void Hide()
    {
        DeselectAllItems();
        Debug.Log("Hide Convoy");
        if(convoy!=null)
            convoy.Deselect();
        context = ConvoyContext.Default;
        canvas.enabled = false;
        charView.Hide();
        OnHide?.Invoke();
    }
    private void OnDestroy()
    {
        Player.Instance.Party.Convoy.convoyUpdated -= UpdateConvoy;
        Player.Instance.Party.Convoy.itemAdded -= AddedItem;
    }
    public void NoneClicked()
    {

        
        if (context == ConvoyContext.SelectRelic)
        {
            
            
            Player.Instance.Party.ActiveUnit.UnEquipRelic();
        }
        else if (context == ConvoyContext.SelectCombatItem)
        {
            Player.Instance.Party.ActiveUnit.UnEquipCombatItem();
        }
        context = ConvoyContext.Default;
        charView.UpdateUnit(Player.Instance.Party.ActiveUnit);
        //Hide();
    }

   
    public void UseItemInBattle()
    {
        Debug.Log("Use Item In Battle");
        var selectedItem = convoy.SelectedItem;
        if (selectedItem == null)
            return;
        
        if (selectedItem.item is ConsumableItem cItem)
        {
            if (cItem.target == ItemTarget.Position)
            {
                new GameplayCommands().SelectItem(cItem);
            }
        }

    }


    public event Action OnHide;
}
