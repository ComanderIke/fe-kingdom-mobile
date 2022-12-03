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
}
public class MerchantEncounterNode : EncounterNode
{
    public Merchant merchant;
  
   
    public MerchantEncounterNode(EncounterNode parent,int depth, int childIndex,string label, string description, Sprite sprite) : base(parent, depth, childIndex, label,description, sprite)
    {
        merchant = new Merchant();
       
        merchant.AddItem(new ShopItem(GameData.Instance.GetItemByName("Health Potion"),Random.Range(2,4)));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomCommonConsumeables(),Random.Range(1,3)));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomCommonConsumeables(),Random.Range(1,2)));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomRareConsumeable()));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomGem()));
        merchant.AddItem(new ShopItem(GameData.Instance.GetRandomGem()));
        //merchant.AddItem(new ShopItem(GameData.Instance.GetRandomPotion(),Random.Range(1,4)));


    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIMerchantController>().Show(this,party);
        
        Continue();
        //GameObject.FindObjectOfType<UIInnController>().Show(Player.Instance.Party);
        Debug.Log("Activate MerchantEncounterNode");
    }
}