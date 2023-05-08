using System;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Items;
using Game.GameActors.Items.Gems;
using Game.GameActors.Players;
using Game.GameInput;
using Game.GUI;
using Game.WorldMapStuff.Model;
using UnityEngine;

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
    private Type typeFilter;
    public void Toogle()
    {
        canvas.enabled =! canvas.enabled;
        state = ConvoeyState.Normal;
      
        if(canvas.enabled)
            UpdateValues();
    }
    public void Show()
    {
        Show(typeof(Item));
    }
    public void Show(Type filter)
    {
        typeFilter = filter;
       
        canvas.enabled = true;
 
        state = typeFilter==typeof(Item)?ConvoeyState.Normal:ConvoeyState.ChooseItem;
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
    public void UpdateValues()
    {
        if (characterCanvas.enabled)
        {
            GetComponent<RectTransform>().anchoredPosition = rightPosition;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = leftPosition;
        }
        this.convoy = Player.Instance.Party.Convoy;
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

        if (state == ConvoeyState.Normal)
        {
          
            for (int i = 0; i < convoy.Items.Count; i++)
            {
                var itemController = CreateItemGameObject(convoy.Items[i], i);
                if (convoy.GetSelectedItem() == convoy.Items[i])
                {
                    itemController.Select();
                }
                else
                {
                    itemController.Deselect();
                }
            }
        }

        if (state == ConvoeyState.ChooseItem)
        {
            Debug.Log("StateChooseItem");
            var sortedList = new List<StockedItem>(convoy.Items);
            sortedList.Sort(delegate(StockedItem x, StockedItem y)
            {
                Debug.Log("Compare: "+x.item.Name+" "+y.item.Name);
                if (x.item == null && y.item == null) return 0;
                if (x.item == null) return -1;
                if (y.item == null) return 1;
                if (x.item is Gem)
                {
                    Debug.Log("X is Relic");
                    if (y.item is Gem)
                    {
                        Debug.Log("Both Relics");
                        return 0;
                    }

                    return -1;
                }

                if (y.item is Gem)
                {
                    Debug.Log("Y is Relic");
                    return 1;
                }
                Debug.Log("No Gem");  
                return 0;
            });
            for (int i = 0; i < sortedList.Count; i++)
            {
                Debug.Log(sortedList[i].item.Name);
                CreateItemGameObject(sortedList[i], i);
            }
        }

        if (state == ConvoeyState.ChooseItem)
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
        enabled = false;
        GetComponent<Canvas>().enabled = enabled;
    }

    public ConvoeyState state;
   
    public void ShowGemOptions(Party party)
    {
        typeFilter = typeof(Gem);
        state = ConvoeyState.ChooseItem;
        UpdateValues();
    }

    public void ItemClicked(StockedItem clickedItem)
    {
        Debug.Log("Item Clicked: "+clickedItem);
        convoy.Select(clickedItem);
        ToolTipSystem.Show(clickedItem.item, transform.position, clickedItem.item.Name, clickedItem.item.Description, clickedItem.item.Sprite);
        UpdateValues();
    }

    public void NoneClicked()
    {
        
    }
}

public enum ConvoeyState
{
    Normal,
    ChooseItem
}