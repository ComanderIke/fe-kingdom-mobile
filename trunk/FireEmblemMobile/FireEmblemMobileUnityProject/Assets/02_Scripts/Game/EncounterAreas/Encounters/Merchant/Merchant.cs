using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;

public class Merchant
{
    public List<ShopItem> shopItems = new List<ShopItem>();
   
    public void AddItem(ShopItem item)
    {
        shopItems.Add(item);
    }

    public void RemoveItem(Item selectedItem)
    {
        ShopItem itemToRemove = null;
        foreach (var shopItem in shopItems)
        {
            if (shopItem.Item == selectedItem)
            {
              
                itemToRemove = shopItem;
                break;
            }
        }

        if (itemToRemove != null)
        {
            itemToRemove.stock--;
            if (itemToRemove.stock <= 0)
                shopItems.Remove(itemToRemove);
        }
        else
        {
            Debug.Log("No item found to remove!");
        }
    }
}