using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameResources;
using UnityEditor.UIElements;
using UnityEngine;

public class Merchant
{
    public List<ShopItem> shopItems = new List<ShopItem>();
    public Sprite merchantFace;
    public string merchantName;

    public Merchant(Sprite merchantFace, string merchantName)
    {
        this.merchantFace = merchantFace;
        this.merchantName = merchantName;
    }
    public float priceMultiplier = 1.0f;
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

    public void Buy(ShopItem selectedItem)
    {
        Player.Instance.Party.Money -=GetCost(selectedItem);
        Player.Instance.Party.Convoy.AddItem(selectedItem.Item);
        RemoveItem(selectedItem.Item);
    }

    public void Sell(ShopItem selectedItem)
    {
        Player.Instance.Party.Money += GetCost(selectedItem);
        Player.Instance.Party.Convoy.RemoveItem(selectedItem.Item);
    }

  
    public int GetCost(ShopItem item)
    {
        if (Player.Instance.Party.Convoy.ContainsItem(GameBPData.Instance.GetMemberCard()))
            priceMultiplier = 0.8f;
        else
        {
            priceMultiplier = 1.0f;
        }
        return (int)(item.cost*priceMultiplier);

    }
}