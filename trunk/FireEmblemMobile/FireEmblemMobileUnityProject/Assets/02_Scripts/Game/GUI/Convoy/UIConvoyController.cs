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
        Battle
    }
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject noneButton;
    [SerializeField] private UIEquipmentController equipmentController;
    [SerializeField] private UICharacterViewController charView;
    [SerializeField] private ClickAndHoldButton contextButton;
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
    private void Start()
    {
        Debug.Log("ConvoyStart");
        contextButton.OnClick -= ContextButtonClicked;
        dropButton.OnClick -= DropButtonClicked;
        contextButton.OnClick += ContextButtonClicked;
        dropButton.OnClick += DropButtonClicked;
    }
    private void Update()
    {
      
        
        if (InputUtility.TouchEnd()&&convoy != null)
        {
            if (!itemClicked&& !contextButton.WasPressingUntilLastFrame&&!dropButton.WasPressingUntilLastFrame)
            {
                Debug.Log("Deselect");
                convoy.Deselect();
          
                UpdateValues();
            }
            else
            {
                Debug.Log("Dont Deselect");
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
    void ContextButtonClicked()
    {
        Debug.Log("ContextClicked: "+context);
        var contextItem = convoy.SelectedItem;
        switch (context)
        {
            case ConvoyContext.SelectRelic:
                if (contextItem != null && contextItem.item is Relic relic)
                {
                    convoy.Deselect();
                    Player.Instance.Party.ActiveUnit.Equip(relic);
                    Hide();
                    
                }

                break;
                
            case ConvoyContext.Default:
                    if (contextItem.item is ConsumableItem consumableItem)
                    {
                        convoy.Deselect();
                        consumableItem.Use(Player.Instance.Party.ActiveUnit, convoy);
                    }
                    break;
            case ConvoyContext.Battle:
                UseItemInBattle();
                break;
        }
    }
  
    public void Show()
    {
        Show(typeof(Item), ConvoyContext.Default);
    }
    public void Show(ConvoyContext context)
    {
        Show(typeof(Item), context);
    }
    public void Show(Type filter, ConvoyContext context)
    {
        this.convoy = Player.Instance.Party.Convoy;
        this.context = context;
        typeFilter = filter;
       
        canvas.enabled = true;
        if(!charView.IsVisible)
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
       // Debug.Log("Item type: "+stockedItem.item.GetType()+" "+typeFilter.IsAssignableFrom(stockedItem.item.GetType()));
        itemController.SetValues(stockedItem, !typeFilter.IsAssignableFrom(stockedItem.item.GetType()));
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
        }
        var selectedItem =  convoy.SelectedItem;
        if (selectedItem != null)
        {
            Debug.Log("Selected  Item : "+selectedItem);
            dropButton.gameObject.SetActive(true);
            if (selectedItem.item is Relic)
            {
                contextButton.gameObject.SetActive(true);
                contextButton.SetBackgroundColor(EquipColor);
                contextButton.SetText("Equip");
            }
            else if (selectedItem.item is ConsumableItem)
            {
                contextButton.gameObject.SetActive(true);
                contextButton.SetBackgroundColor(UseColor);
                contextButton.SetText("Use");
            }
        }
        else
        {
            contextButton.gameObject.SetActive(false);
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
            instantiatedItems = new List<GameObject>();
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
            Debug.Log("Compare: "+x.item.Name+" "+y.item.Name);
            if (x.item == null && y.item == null) return 0;
            if (x.item == null) return -1;
            if (y.item == null) return 1;
            if (x.item.GetType()== typeFilter)
            {
                Debug.Log("X is Relic");
                
                if (y.item.GetType() == typeFilter)
                {
                    Debug.Log("Both Relics");
                    return 0;
                }

                return -1;
            }

            if (y.item.GetType() == typeFilter)
            {
                Debug.Log("Y is Relic");
                return 1;
            }
            Debug.Log("No Gem");  
            return 0;
        });

        for (int i = 0; i < sortedList.Count; i++)
        {
            var itemController = CreateItemGameObject(sortedList[i], i);
            if (convoy.SelectedItem == sortedList[i])
            {
                itemController.Select();
            }
            else
            {
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
    private void ItemClicked(UIConvoyItemController clickedItem)
    {
        itemClicked = true;
        Debug.Log("Item Clicked: "+clickedItem);
        convoy.Select(clickedItem.stockedItem);
        ToolTipSystem.Show(clickedItem.stockedItem.item, clickedItem.transform.position, clickedItem.stockedItem.item.Name, clickedItem.stockedItem.item.Description, clickedItem.stockedItem.item.Sprite);
        UpdateValues();
    }
    public void Hide()
    {
        convoy.Deselect();
        context = ConvoyContext.Default;
        canvas.enabled = false;
        charView.Hide();
    }
    private void OnDestroy()
    {
        Player.Instance.Party.Convoy.convoyUpdated -= UpdateConvoy;
    }
    public void NoneClicked()
    {
        if (context == ConvoyContext.SelectRelic)
        {
            Player.Instance.Party.ActiveUnit.UnEquipRelic(equipmentController.selectedSlotNumber);
        }
        Hide();
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
    void RelicEquipClicked(Relic relic)
    {
      
        Unit human =Player.Instance.Party.ActiveUnit;
        if (equipmentController.selectedSlot == null)
        {
            if(!charView.IsVisible)
                charView.Show(human);
            equipmentController.HighlightRelicSlots();
        }
        else
        {
            EquipRelicOnSelectedSlot(human, relic);
        }
        
    }

  
    void EquipRelicOnSelectedSlot(Unit human, Relic relic)
    {
        var equippedRelic = equipmentController.selectedSlotNumber == 1 ? human.EquippedRelic1 : null;
        if (equippedRelic == null)
            equippedRelic = equipmentController.selectedSlotNumber == 2 ? human.EquippedRelic2 : null;
        
        if (human.HasEquipped(relic))
        {
            human.UnEquip((relic));
            Player.Instance.Party.Convoy.AddItem(relic);
        }
        else
        {
            if(equippedRelic!=null)
                Player.Instance.Party.Convoy.AddItem(equippedRelic);
            human.Equip((relic), equipmentController.selectedSlotNumber);
            Player.Instance.Party.Convoy.RemoveItem(relic);
        }
    }

   
}
