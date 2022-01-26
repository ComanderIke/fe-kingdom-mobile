using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class Merchant
{
    public List<EquipableItem> shopItems = new List<EquipableItem>();
   
    public void AddItem(EquipableItem item)
    {
        shopItems.Add(item);
    }
}
public class MerchantEncounterNode : EncounterNode
{
    public Merchant merchant;
   
    public MerchantEncounterNode(EncounterNode parent) : base(parent)
    {
        merchant = new Merchant();
        merchant.AddItem(GameData.Instance.GetRandomPotion());
        merchant.AddItem(GameData.Instance.GetRandomPotion());
        merchant.AddItem(GameData.Instance.GetRandomPotion());
        merchant.AddItem(GameData.Instance.GetRandomMagic());
        merchant.AddItem(GameData.Instance.GetRandomMagic());
        merchant.AddItem(GameData.Instance.GetRandomWeapon());
        
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIMerchantController>().Show(this,party);
        //GameObject.FindObjectOfType<UIInnController>().Show(Player.Instance.Party);
        Debug.Log("Activate MerchantEncounterNode");
    }
}