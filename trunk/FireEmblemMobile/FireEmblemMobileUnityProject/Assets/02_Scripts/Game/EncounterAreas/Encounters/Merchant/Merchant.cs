using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameResources;
using UnityEngine;

public class Merchant
{
    public List<StockedItem> shopItems = new List<StockedItem>();
    public Sprite merchantFace;
    public string merchantName;

    public Merchant(Sprite merchantFace, string merchantName)
    {
        if (PriceRateSelling == 0)
            PriceRateSelling = .5f;
        if (PriceRateBuying == 0)
            PriceRateBuying = 1.0f;
        this.merchantFace = merchantFace;
        this.merchantName = merchantName;
    }
    public float priceMultiplier = 1.0f;
    public static float PriceRateSelling { get; set; }
    public static float PriceRateBuying { get; set; }
    public static int SlotCount = 4;

    public void AddItem(StockedItem item)
    {
        shopItems.Add(item);
    }

    public void RemoveItem(Item selectedItem)
    {
        StockedItem itemToRemove = null;
        foreach (var shopItem in shopItems)
        {
            if (shopItem.item == selectedItem)
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

    public void Buy(Item selectedItem)
    {
        Player.Instance.Party.Money -=(int)(GetCost(selectedItem));
        Player.Instance.Party.AddItem(selectedItem);
        RemoveItem(selectedItem);
    }

    public void Sell(Item selectedItem)
    {
        Player.Instance.Party.Money += (int)(GetSellCost(selectedItem));
        Player.Instance.Party.RemoveItem(selectedItem);
    }

    public int GetSellCost(Item item)
    {
        priceMultiplier = PriceRateSelling;
        return (int)(item.cost*priceMultiplier);

    }
    
  
    public int GetCost(Item item)
    {
        if (Player.Instance.Party.Convoy.ContainsItem(GameBPData.Instance.GetMemberCard()))
            priceMultiplier = 0.8f;
        else
        {
            priceMultiplier = PriceRateBuying;
        }
        return (int)(item.cost*priceMultiplier);

    }

    public void GenerateItems()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            if(i==0)
                AddItem(new StockedItem(GameBPData.Instance.GetItemByName("Health Potion"), Random.Range(1,4)));
            else if(i==1||i==6)
                AddItem(GameBPData.Instance.GetRandomMerchantItemSingularStock());
            else
            {
                AddItem(GameBPData.Instance.GetRandomMerchantItemMultipleStock());
            }
        }
    }
}