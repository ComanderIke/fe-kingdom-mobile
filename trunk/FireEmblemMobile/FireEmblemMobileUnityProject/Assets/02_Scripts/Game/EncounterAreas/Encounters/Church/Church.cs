using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameResources;
using LostGrace;
using UnityEngine;

public class Church
{
    public List<ShopItem> shopItems = new List<ShopItem>();
    private IBlessingData blessingData;
    public Church(IBlessingData blessingData)
    {
        this.blessingData = blessingData;
    }
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

    Blessing GenerateBlessing(int faith, int donation)
    {
       
        Blessing ret;
        List<Blessing> tier0BlessingPool= blessingData.GetBlessingPool(0);//400 points
        List<Blessing> tier1BlessingPool= blessingData.GetBlessingPool(1);//300
        List<Blessing> tier2BlessingPool= blessingData.GetBlessingPool(2);//200
        List<Blessing> tier3BlessingPool= blessingData.GetBlessingPool(3);//100
        List<Blessing> tier4BlessingPool= blessingData.GetBlessingPool(4);//
        int faithTierIncreasePercent = faith * 10;
        //100 points=upgrade in tier so 10 Faith +1 Tier guarantued
        //10 FAith = 50%
        //+donate High ~100%
        ret = tier0BlessingPool[0];
        return ret;
    }
    public Blessing DonateSmall(int faith)
    {
        return GenerateBlessing(faith,25);
        //lowtier 0-25%
    }

    public Blessing DonateMedium(int faith)
    {
        return GenerateBlessing(faith,100);
    }

    public Blessing DonateHigh(int faith)
    {
        return GenerateBlessing(faith,200);
    }
}

public interface IBlessingData
{
    List<Blessing> GetBlessingPool(int p0);
}