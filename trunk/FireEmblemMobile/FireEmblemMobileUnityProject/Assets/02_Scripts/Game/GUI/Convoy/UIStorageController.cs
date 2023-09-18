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
        itemController.SetValues(stockedItem, false);
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
                itemController.Select();
            }
            else
            {
                itemController.Deselect();
            }
        }
 
    }
    private void UpdateStorage()
    {
        Debug.Log("Update Convoy!");
        UpdateValues();
    }
 
    private void ItemClicked(UIConvoyItemController clickedItem)
    {
        Debug.Log("Item Clicked: "+clickedItem);
        storage.Select(clickedItem.stockedItem);
        ToolTipSystem.Show(clickedItem.stockedItem.item, clickedItem.transform.position);
        UpdateValues();
        
        
    }
    public void Hide()
    {
        Debug.Log("Hide Convoy");
        storage.Deselect();
        canvas.enabled = false;
        OnHide?.Invoke();
    }
    private void OnDestroy()
    {
        Player.Instance.Party.Storage.convoyUpdated -= UpdateStorage;
    }

    public event Action OnHide;
}