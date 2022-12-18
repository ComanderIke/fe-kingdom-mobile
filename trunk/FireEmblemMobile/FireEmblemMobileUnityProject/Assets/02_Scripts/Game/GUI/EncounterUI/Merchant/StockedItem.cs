using System;
using Game.GameActors.Items;
using UnityEngine.Serialization;

[Serializable]
public class StockedItem
{
    [FormerlySerializedAs("itemBp")] public Item item;
    public int stock = 1;

    public StockedItem(Item item,int stock)
    {
        this.stock = stock;
        this.item = item;
    }

   

   
}