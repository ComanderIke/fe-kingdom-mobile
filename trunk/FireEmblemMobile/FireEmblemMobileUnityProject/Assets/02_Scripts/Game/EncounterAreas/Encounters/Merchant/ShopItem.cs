using System;
using Game.GameActors.Items;
using UnityEngine;

public class ShopItem:IEquatable<ShopItem>
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

    public bool Equals(ShopItem other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return name == other.name && cost == other.cost && Equals(sprite, other.sprite) && description == other.description && stock == other.stock && Equals(Item, other.Item);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ShopItem)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (name != null ? name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ cost;
            hashCode = (hashCode * 397) ^ (sprite != null ? sprite.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (description != null ? description.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ stock;
            hashCode = (hashCode * 397) ^ (Item != null ? Item.GetHashCode() : 0);
            return hashCode;
        }
    }
}