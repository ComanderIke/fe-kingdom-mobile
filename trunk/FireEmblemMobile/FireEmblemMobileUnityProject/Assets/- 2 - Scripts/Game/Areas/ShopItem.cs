using UnityEngine;

public class ShopItem
{
    public int cost;
    public Sprite sprite;
    public string description;

    public ShopItem(int cost, Sprite sprite, string description)
    {
        this.cost = cost;
        this.sprite = sprite;
        this.description = description;
    }
}