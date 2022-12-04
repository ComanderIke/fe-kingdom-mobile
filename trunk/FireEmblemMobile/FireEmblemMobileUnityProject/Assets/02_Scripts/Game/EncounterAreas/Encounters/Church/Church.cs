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
    private Dictionary<Unit, Blessing> alreadyGeneratedBlessing;
    private Dictionary<Unit, Blessing> alreadyAcceptedBlessing;
    private IBlessingData blessingData;
    public Church(IBlessingData blessingData)
    {
        this.blessingData = blessingData;
        alreadyDonated = new List<Unit>();
        alreadyAcceptedBlessing = new Dictionary<Unit, Blessing>();
        alreadyGeneratedBlessing = new Dictionary<Unit, Blessing>();
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
        unit.Party.Money -= 25;
        var blessing=GenerateBlessing(faith,25);
        alreadyGeneratedBlessing.Add(unit, blessing);
        return blessing;
        //lowtier 0-25%
    }

    public Blessing DonateMedium(Unit unit, int faith)
    {
        alreadyDonated.Add(unit);
        unit.Party.Money -= 50;
        var blessing =GenerateBlessing(faith, 100);
        alreadyGeneratedBlessing.Add(unit, blessing);
        return blessing;
    }

    public Blessing DonateHigh(Unit unit, int faith)
    {
        alreadyDonated.Add(unit);
        unit.Party.Money -= 100;
        var blessing = GenerateBlessing(faith, 200);
        alreadyGeneratedBlessing.Add(unit, blessing);
        return blessing;
    }

   
    public bool CanDonate(Unit unit)
    {
        return !alreadyDonated.Contains(unit);
    }

    public void BlessUnit(Unit unit, Blessing blessing)
    {
        unit.ReceiveBlessing(blessing);
        alreadyAcceptedBlessing.Add(unit, blessing);
    }

    public bool AlreadyAcceptedBlessing(Unit unit)
    {
        return alreadyAcceptedBlessing.ContainsKey(unit);
    }

    public Blessing GetAlreadyAcceptedBlessing(Unit unit)
    {
        return alreadyAcceptedBlessing[unit];
    }
    public bool AlreadyGeneratedBlessing(Unit unit)
    {
        return alreadyGeneratedBlessing.ContainsKey(unit);
    }

    public Blessing GetAlreadyGeneratedBlessing(Unit unit)
    {
        return alreadyGeneratedBlessing[unit];
    }
}

public interface IBlessingData
{
    BlessingBP[]  GetBlessingPool(int p0);
}