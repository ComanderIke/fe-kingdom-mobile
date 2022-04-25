using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;


public class Merchant
{
    public List<ShopItem> shopItems = new List<ShopItem>();
   
    public void AddItem(ShopItem item)
    {
        shopItems.Add(item);
    }

    public void RemoveItem(Item selectedItem)
    {
        ShopItem itemToRemove = null;
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
}
public class MerchantEncounterNode : EncounterNode
{
    public Merchant merchant;
  
   
    public MerchantEncounterNode(EncounterNode parent,int depth, int childIndex) : base(parent, depth, childIndex)
    {
        merchant = new Merchant();
       
        merchant.AddItem(new ShopItem(GameData.Instance.GetHealthPotion(),Random.Range(2,4)));
        merchant.AddItem(new ShopItem(GameData.Instance.GetSPotion(),Random.Range(2,4)));
        //merchant.AddItem(new ShopItem(GameData.Instance.GetRandomPotion(),Random.Range(1,4)));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomMagic()));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomArmor()));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomRelic()));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomWeapon()));
        
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIMerchantController>().Show(this,party);
        
        Continue();
        //GameObject.FindObjectOfType<UIInnController>().Show(Player.Instance.Party);
        Debug.Log("Activate MerchantEncounterNode");
    }
}