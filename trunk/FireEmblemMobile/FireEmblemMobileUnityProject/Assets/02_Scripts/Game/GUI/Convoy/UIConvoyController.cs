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
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class UIConvoyController:MonoBehaviour
{
    [SerializeField] private Canvas canvas;

   
    public GameObject convoyItemPrefab;
    public List<ConvoyDropArea> DropAreas;

    private List<GameObject> instantiatedItems;
    private Convoy convoy;
    private bool init = false;
    public Canvas characterCanvas;
    public Vector3 leftPosition;
    public Vector3 rightPosition;
    [SerializeField] private GameObject noneButton;
    [SerializeField] private UIEquipmentController equipmentController;
    [SerializeField] private UICharacterViewController charView;
    [SerializeField] private ClickAndHoldButton contextButton;
    [SerializeField] private ClickAndHoldButton dropButton;
    [SerializeField] private TextMeshProUGUI contextText;
    private Type typeFilter;
    private ConvoyContext context;
    
   

    [SerializeField] private Color EquipColor;
    [SerializeField] private Color UseColor;
    [SerializeField] private Color DropColor;


    public void Toogle()
    {
        if(canvas.enabled)
            Hide();
        else
        {
            Show();
        }
    }

    private void Start()
    {
        contextButton.OnClick -= ContextButtonClicked;
        dropButton.OnClick -= DropButtonClicked;
        contextButton.OnClick += ContextButtonClicked;
        dropButton.OnClick += DropButtonClicked;
    }
    void DropButtonClicked()
    {
        convoy.RemoveStockedItem(convoy.SelectedItem);
    }
    void ContextButtonClicked()
    {
        switch (context)
        {
            case ConvoyContext.SelectRelic:
                if (convoy.SelectedItem != null && convoy.SelectedItem.item is Relic relic)
                {
                    Player.Instance.Party.ActiveUnit.Equip(relic);
                }
                
                break;
        }
    }
    public enum ConvoyContext
    {
        Default,
        SelectRelic,
        ChooseGem
    }
    public void Show()
    {
        Show(typeof(Item), ConvoyContext.Default);
    }
    public void Show(Type filter, ConvoyContext context)
    {
        this.convoy = Player.Instance.Party.Convoy;
        this.context = context;
        typeFilter = filter;
       
        canvas.enabled = true;

        if (typeFilter == typeof(Gem))
        {
            context = ConvoyContext.ChooseGem;
        }
        UpdateValues();
        //Debug.Log("Showing convoy! itemcount: " + convoy.Items.Count);
    }

    public UIConvoyItemController CreateItemGameObject(StockedItem stockedItem, int index)
    {
        if(typeFilter==null)
           typeFilter = typeof(Item);
        var go = Instantiate(convoyItemPrefab, DropAreas[index].transform);
        var itemController = go.GetComponent<UIConvoyItemController>();
       // Debug.Log("Item type: "+stockedItem.item.GetType()+" "+typeFilter.IsAssignableFrom(stockedItem.item.GetType()));
        itemController.SetValues(stockedItem, !typeFilter.IsAssignableFrom(stockedItem.item.GetType()));
        itemController.onClicked += ItemClicked;
        var dragController = go.GetComponent<UIDragable>();
        dragController.SetItem(stockedItem.item);
        dragController.SetCanvas(GetComponent<Canvas>());
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
    public void UpdateValues()
    {
       
        UpdateContext();
     
        // if (characterCanvas.enabled)
        // {
        //     GetComponent<RectTransform>().anchoredPosition = rightPosition;
        // }
        // else
        // {
        //     GetComponent<RectTransform>().anchoredPosition = leftPosition;
        // }
        
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

    private void OnDestroy()
    {
        Player.Instance.Party.Convoy.convoyUpdated -= UpdateConvoy;
    }

    private void UpdateConvoy()
    {
        UpdateValues();
    }
    public void Hide()
    {
        convoy.Deselect();
        context = ConvoyContext.Default;
        canvas.enabled = false;
    }

   
    public void ShowGemOptions(Party party)
    {
        typeFilter = typeof(Gem);
        context = ConvoyContext.ChooseGem;
        UpdateValues();
    }

    private bool itemClicked = false;
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

    public void ItemClicked(UIConvoyItemController clickedItem)
    {
        itemClicked = true;
        Debug.Log("Item Clicked: "+clickedItem);
        convoy.Select(clickedItem.stockedItem);
        ToolTipSystem.Show(clickedItem.stockedItem.item, clickedItem.transform.position, clickedItem.stockedItem.item.Name, clickedItem.stockedItem.item.Description, clickedItem.stockedItem.item.Sprite);
        UpdateValues();
    }
    public void UseClicked()
    {
 
        var selectedItem = convoy.SelectedItem;
        if (selectedItem == null)
            return;

      
        if (selectedItem.item is Relic eitem)
        {
            RelicEquipClicked(eitem);
        }
        else 
        {
            if (selectedItem.item is ConsumableItem cItem)
            {
                cItem.Use(Player.Instance.Party.ActiveUnit, Player.Instance.Party.Convoy);
            }
        }
    }
    void RelicEquipClicked(Relic relic)
    {
      
        Unit human =Player.Instance.Party.ActiveUnit;
        if (equipmentController.selectedSlot == null)
        {
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

    public void NoneClicked()
    {
        if (context == ConvoyContext.SelectRelic)
        {
            Player.Instance.Party.ActiveUnit.UnEquipRelic(equipmentController.selectedSlotNumber);
        }
        Hide();
    }
}
