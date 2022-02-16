using UnityEngine;

public class ShopItem
{
    public string name;
    public int cost;
    public Sprite sprite;
    public string description;

    public ShopItem(string name, int cost, Sprite sprite, string description)
    {
        this.name = name;
        this.cost = cost;
        this.sprite = sprite;
        this.description = description;
    }
}