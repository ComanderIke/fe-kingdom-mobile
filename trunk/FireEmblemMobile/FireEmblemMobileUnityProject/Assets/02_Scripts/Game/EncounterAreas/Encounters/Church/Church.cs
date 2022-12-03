using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Units;
using Game.GameResources;
using LostGrace;
using UnityEngine;
using Random = System.Random;

public class Church
{
    public List<ShopItem> shopItems = new List<ShopItem>();
    private List<Unit> alreadyDonated;
    private IBlessingData blessingData;
    public Church(IBlessingData blessingData)
    {
        this.blessingData = blessingData;
        alreadyDonated = new List<Unit>();
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
        BlessingBP[] tier0BlessingPool= blessingData.GetBlessingPool(0);//400 points
        BlessingBP[] tier1BlessingPool= blessingData.GetBlessingPool(1);//300
        BlessingBP[]  tier2BlessingPool= blessingData.GetBlessingPool(2);//200
        BlessingBP[]  tier3BlessingPool= blessingData.GetBlessingPool(3);//100
        int faithTierIncreasePercent = faith * 10;
        //100 points=upgrade in tier so 10 Faith +1 Tier guarantued
        //10 FAith = 50%
        //+donate High ~100%
        ret = tier3BlessingPool[UnityEngine.Random.Range(0, tier3BlessingPool.Length)].Create();
        return ret;
    }
    public Blessing DonateSmall(Unit unit, int faith)
    {
        alreadyDonated.Add(unit);
        return GenerateBlessing(faith,25);
        //lowtier 0-25%
    }

    public Blessing DonateMedium(Unit unit, int faith)
    {
        alreadyDonated.Add(unit);
        return GenerateBlessing(faith, 100);
    }

    public Blessing DonateHigh(Unit unit, int faith)
    {
        alreadyDonated.Add(unit);
        return GenerateBlessing(faith, 200);
    }

    public bool CanDonate(Unit unit)
    {
        return !alreadyDonated.Contains(unit);
    }
}

public interface IBlessingData
{
    BlessingBP[]  GetBlessingPool(int p0);
}