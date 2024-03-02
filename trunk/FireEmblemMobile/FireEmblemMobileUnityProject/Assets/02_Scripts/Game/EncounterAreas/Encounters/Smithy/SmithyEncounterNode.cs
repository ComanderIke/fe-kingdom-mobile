using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class Smithy
{
    private List<int> upgradeGoldCost = new List<int>(){ 100,200,300 };
    private List<int> upgradeStoneCost = new List<int>(){ 0,1,0 };
    private List<int> upgradeDragonScaleCost = new List<int>(){ 0,0,1 };


    public List<EquipableItem> shopItems = new List<EquipableItem>();
    public static float PriceRate = 1.0f;
    public static int MaxUpgradeLevel { get; set; }
    public static int GemStoneMergeAmount = 4;

    public void AddItem(EquipableItem item)
    {
        shopItems.Add(item);
    }

    public int GetGoldUpgradeCost(Weapon equippedWeapon)
    {
        return (int)(upgradeGoldCost[equippedWeapon.weaponLevel - 1]*PriceRate);
    }

    public int GetStoneUpgradeCost(Weapon equippedWeapon)
    {
        return upgradeStoneCost[equippedWeapon.weaponLevel - 1];
    }

    public int GetDragonScaleUpgradeCost(Weapon equippedWeapon)
    {
        return upgradeDragonScaleCost[equippedWeapon.weaponLevel - 1];
    }
}
public class SmithyEncounterNode : EncounterNode
{
    public Smithy smithy;

    public SmithyEncounterNode(List<EncounterNode> parents,int depth, int childIndex,string label, string description, Sprite sprite) : base(parents, depth, childIndex,label, description, sprite)
    { 
        smithy = new Smithy();
        smithy.AddItem(GameBPData.Instance.GetRandomBow());
        smithy.AddItem(GameBPData.Instance.GetRandomSpear());
        smithy.AddItem(GameBPData.Instance.GetRandomSword());
        smithy.AddItem(GameBPData.Instance.GetRandomSword());
    

    }

    public override void Activate(Party party)
    {
        base.Activate(party);
        if(Player.Instance.Flags.SmithingBonds)
            foreach (var member in party.members)
            {
                member.Bonds.Increase(GameBPData.Instance.GetGod("Hephaestus"),10);
            }
        GameObject.FindObjectOfType<UISmithyController>().Show(this,party);
        MyDebug.LogLogic("Visiting Smithy");
    }
}