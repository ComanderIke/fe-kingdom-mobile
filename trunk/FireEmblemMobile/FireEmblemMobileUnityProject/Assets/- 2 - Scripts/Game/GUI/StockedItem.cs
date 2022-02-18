using System;
using Game.GameActors.Items;

[Serializable]
public class StockedItem
{
    public Item item;
    public int stock = 1;

    public StockedItem(Item item,int stock)
    {
        this.stock = stock;
        this.item = item;
    }
}