﻿using Game.GameActors.Items;
using UnityEngine;

public class ShopItem
{
    public string name;
    public int cost;
    public Sprite sprite;
    public string description;
    public int stock;
    public Item Item;

  
    public ShopItem(Item item, int stock = 1)
    {
        this.Item = item;
        this.name = item.Name;
        this.cost = item.cost;
        this.sprite = item.Sprite;
        this.description = item.Description;
        this.stock = stock;
    }
}