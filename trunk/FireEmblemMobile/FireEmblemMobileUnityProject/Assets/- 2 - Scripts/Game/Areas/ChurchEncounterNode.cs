using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameResources;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class Church
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
public class ChurchEncounterNode : EncounterNode
{
    public Church church;

    public ChurchEncounterNode(EncounterNode parent,int depth, int childIndex) : base(parent, depth, childIndex)
    {
        church = new Church();
        church.AddItem(new ShopItem(GameData.Instance.GetRandomRelic()));
        church.AddItem(new ShopItem(GameData.Instance.GetRandomMagic()));
        //church.AddItem(new ShopItem(GameData.Instance.GetRandomStaff()));
       
    }

    public override void Activate(Party party)
    {
        GameObject.FindObjectOfType<UIChurchController>().Show(this,party);
        Debug.Log("Activate ChurchEncounterNode");
    }
}