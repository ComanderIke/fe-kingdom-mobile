using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Manager;
using LostGrace;
using UnityEngine;
using Random = System.Random;

public class Church
{
  //  public List<ShopItem> shopItems = new List<ShopItem>();
    private List<Unit> alreadyDonated;
    private Dictionary<Unit, Blessing> alreadyGeneratedBlessing;
    private Dictionary<Unit, Blessing> alreadyAcceptedBlessing;
    private IBlessingData blessingData;
    private const int ChurchDonationSmallCost = 25;
    private const int ChurchDonationMiddleCost = 50;
    private const int ChurchDonationHighCost= 100;
    private const int ChurchDonationSmallAmount = 25;
    private const int ChurchDonationMiddleAmount = 100;
    private const int ChurchDonationHighAmount = 200;
    public Church(IBlessingData blessingData)
    {
        this.blessingData = blessingData;
        alreadyDonated = new List<Unit>();
        alreadyAcceptedBlessing = new Dictionary<Unit, Blessing>();
        alreadyGeneratedBlessing = new Dictionary<Unit, Blessing>();
    }

    public static float PriceRate { get; set; }
    // public void AddItem(ShopItem item)
    // {
    //     shopItems.Add(item);
    // }

    // public void RemoveItem(Item selectedItem)
    // {
    //     ShopItem itemToRemove = null;
    //     foreach (var shopItem in shopItems)
    //     {
    //         if (shopItem.Item == selectedItem)
    //         {
    //           
    //             itemToRemove = shopItem;
    //             break;
    //         }
    //     }
    //
    //     if (itemToRemove != null)
    //     {
    //         itemToRemove.stock--;
    //         if (itemToRemove.stock <= 0)
    //             shopItems.Remove(itemToRemove);
    //     }
    //     else
    //     {
    //         Debug.Log("No item found to remove!");
    //     }
    // }

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
        //TODO calculate blessing tier
        ret =(Blessing) tier3BlessingPool[UnityEngine.Random.Range(0, tier3BlessingPool.Length)].Create();
        
        return ret;
    }

    private float blessingUpgradeChance = .1f;
    private int maxUpgrades = 2;
    // void ShowBlessings(int faith, int donation)
    // {
    //     Blessing blessing1 = null;
    //     var playerBlessing = Player.Instance.Party.ActiveUnit.SkillManager.GetBlessing();
    //     if (playerBlessing!=null&& playerBlessing.Upgradable())
    //     {
    //         blessing1 = (Blessing)playerBlessing.Clone();
    //         blessing1.Level++;
    //     }
    //     else{
    //         blessing1 = GenerateBlessing(faith, donation);
    //     }
    //     var blessing2 = GenerateBlessing(faith, donation);
    //     var blessing3 = GenerateBlessing(faith, donation);
    //     ServiceProvider.Instance.GetSystem<SkillSystem>().LearnNewSkill(Player.Instance.Party.ActiveUnit, blessing1, blessing2, blessing3);
    // // }
    // public void DonateSmall(Unit unit, int faith)
    // {
    //     alreadyDonated.Add(unit);
    //     unit.Party.Money -= ChurchDonationSmallCost;
    //    // ShowBlessings(faith, ChurchDonationSmallAmount);
    // }
    //
    // public void DonateMedium(Unit unit, int faith)
    // {
    //     alreadyDonated.Add(unit);
    //     unit.Party.Money -= ChurchDonationMiddleCost;
    //    // ShowBlessings(faith, ChurchDonationMiddleAmount);
    //     
    // }
    //
    // public void DonateHigh(Unit unit, int faith)
    // {
    //     Debug.Log("DonateHigh");
    //     alreadyDonated.Add(unit);
    //     unit.Party.Money -= ChurchDonationHighCost;
    //     ShowBlessings(faith, ChurchDonationHighAmount);
    // }

   
    public bool CanDonate(Unit unit)
    {
        return !alreadyDonated.Contains(unit);
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

    public bool AlreadyRemovedCurse(Unit partyActiveUnit)
    {
        throw new System.NotImplementedException();
    }
}

public interface IBlessingData
{
    BlessingBP[]  GetBlessingPool(int p0);
}