using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Manager;
using Game.WorldMapStuff.Model;
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
    private God god;
    
    public Church(IBlessingData blessingData)
    {
        this.blessingData = blessingData;
        List<God> gods= GameBPData.Instance.GetAllGods().ToList();
        
        god = gods[UnityEngine.Random.Range(0, gods.Count())];
        alreadyDonated = new List<Unit>();
        alreadyAcceptedBlessing = new Dictionary<Unit, Blessing>();
        alreadyGeneratedBlessing = new Dictionary<Unit, Blessing>();
        alreadyPrayed = new List<Unit>();
    }

    public God GetGod()
    {
        return god;
    }

    public static float PriceRate = 1.0f;

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
    [SerializeField] private float faithExpMult = 5f;
    [SerializeField] private List<Unit> alreadyPrayed;
    public void Pray(Unit unit, God god)
    {
        alreadyPrayed.Add(unit);
        unit.Bonds.Increase(god, (int)(unit.Stats.CombinedAttributes().FAITH*faithExpMult));

    }

    public bool AlreadyPrayed(Unit partyActiveUnit)
    {
        return alreadyPrayed.Contains(partyActiveUnit);
    }
    private int donateExtraExp = 50;
    private int goldCost = 100;
    public void Donate(Party party, Unit partyActiveUnit, God god)
    {
        party.AddGold(-DonateCost());
        partyActiveUnit.Bonds.Increase(god,donateExtraExp);
    }

    public int DonateCost()
    {
        return (int)(goldCost* PriceRate);
    }
}

public interface IBlessingData
{
    BlessingBP[]  GetBlessingPool(int p0);
}