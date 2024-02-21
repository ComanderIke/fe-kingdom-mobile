using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI;
using Game.Utility;
using Game.WorldMapStuff.Model;
using LostGrace;
using TMPro;
using UnityEngine;

public class UIStorageController : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public GameObject convoyItemPrefab;
   private List<GameObject> instantiatedItems;
    
    public List<ConvoyDropArea> DropAreas;
    private Convoy storage;
    private bool init = false;
    private float timeShown = 0;
  
    private void Update()
    {
        if(!canvas.enabled)
          return;
        timeShown += Time.deltaTime;
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
    void DeselectAllItems()
    {
        foreach (var dropArea in DropAreas)
        {
            dropArea.Deselect();
        }
    }
    public void Show()
    {
        
        timeShown = 0;
        this.storage = Player.Instance.Party.Storage;
        canvas.enabled = true;
      
        UpdateValues();
        //Debug.Log("Showing convoy! itemcount: " + convoy.Items.Count);
    }

    private UIConvoyItemController CreateItemGameObject(StockedItem stockedItem, int index)
    {
        var go = Instantiate(convoyItemPrefab, DropAreas[index].transform);
        var itemController = go.GetComponent<UIConvoyItemController>();
        bool isNewItem = newItems.Contains(stockedItem);
        Debug.Log("Create Item: "+stockedItem.item+" isNew: "+isNewItem);
        itemController.SetValues(stockedItem, index, isNewItem,false);
        if(isNewItem)
            newItems.Remove(stockedItem);
        itemController.onClicked += ItemClicked;
 
        instantiatedItems.Add(go);
        return itemController;
    }

   
    
    private void UpdateValues()
    {
        if (!init)
        {
            init = true;
            Player.Instance.Party.Storage.convoyUpdated += UpdateStorage;
            Player.Instance.Party.Storage.itemAdded += AddedItem;
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
        
        var sortedList = new List<StockedItem>(storage.Items);
        sortedList.Sort(delegate(StockedItem x, StockedItem y)
        {
           // Debug.Log("Compare: "+x.item.Name+" "+y.item.Name);
            if (x.item == null && y.item == null) return 0;
            if (x.item == null) return -1;
            if (y.item == null) return 1;
           

            Debug.Log("No Gem");  
            return 0;
        });

        for (int i = 0; i < sortedList.Count; i++)
        {
            var itemController = CreateItemGameObject(sortedList[i], i);
            if (storage.SelectedItem == sortedList[i])
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
 
    }

    private List<StockedItem> newItems;
    void AddedItem(StockedItem item)
    {
        Debug.Log("Added item: "+item.item);
        newItems.Add(item);
        UpdateValues();
    }
    private void UpdateStorage()
    {
        Debug.Log("Update Convoy!");
        UpdateValues();
    }

    private int currentSelected = 0;
 
    private void ItemClicked(UIConvoyItemController clickedItem)
    {
        Debug.Log("Item Clicked: "+clickedItem);
        storage.Select(clickedItem.stockedItem);
        // DropAreas[currentSelected].Deselect();
        // currentSelected = clickedItem.index;
        // DropAreas[currentSelected].Select();
        //ToolTipSystem.Show(clickedItem.stockedItem, clickedItem.transform.position);
        UpdateValues();
        
        
    }
    public void Hide()
    {
        MyDebug.LogUI("Hide Convoy");
        DeselectAllItems();
        if(storage!=null)
            storage.Deselect();
        canvas.enabled = false;
        OnHide?.Invoke();
    }

   
    private void OnDestroy()
    {
        Player.Instance.Party.Storage.convoyUpdated -= UpdateStorage;
        Player.Instance.Party.Storage.itemAdded -= AddedItem;
    }

    public event Action OnHide;
}