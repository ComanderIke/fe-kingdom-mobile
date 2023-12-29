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
    public static float PriceRate { get; set; }
    public static int MaxUpgradeLevel { get; set; }

    public void AddItem(EquipableItem item)
    {
        shopItems.Add(item);
    }

    public int GetGoldUpgradeCost(Weapon equippedWeapon)
    {
        return upgradeGoldCost[equippedWeapon.weaponLevel - 1];
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
        GameObject.FindObjectOfType<UISmithyController>().Show(this,party);
        Debug.Log("Activate SmithyEncounterNode");
    }
}