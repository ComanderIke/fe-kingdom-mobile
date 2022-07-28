using System;
using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using TMPro;
using UnityEngine;

public interface IShopItemClickedReceiver
{
    void ItemClicked(ShopItem item);
}
public class UIShopItemController : UIButtonController
{
    public Color tooExpensiveColor;
    public Color normalColor;
    private ShopItem item;
    public TextMeshProUGUI stockCount;
    private IShopItemClickedReceiver clickedReceiver;
    public void SetValues(ShopItem item, bool affordable,IShopItemClickedReceiver receiver)
    {
        this.item = item;
        if (affordable)
            cost.color = normalColor;
        else
        {
            cost.color = tooExpensiveColor;
        }

        this.clickedReceiver = receiver;
        SetValues(item.sprite, item.cost, item.description);
        stockCount.text = "" + item.stock+"x";
        stockCount.gameObject.SetActive(item.stock > 1);
    }

    public void Clicked()
    {
        Debug.Log("ItemClicked!" + item.name);
        clickedReceiver.ItemClicked(item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }
}
