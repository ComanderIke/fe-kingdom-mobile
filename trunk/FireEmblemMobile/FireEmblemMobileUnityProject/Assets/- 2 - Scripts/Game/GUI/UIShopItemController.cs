using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShopItemController : UIButtonController
{
    public Color tooExpensiveColor;
    public Color normalColor;
    private ShopItem item;
    public TextMeshProUGUI stockCount;
    public void SetValues(ShopItem item, bool affordable)
    {
        this.item = item;
        if (affordable)
            cost.color = normalColor;
        else
        {
            cost.color = tooExpensiveColor;
        }
        SetValues(item.sprite, item.cost, item.description);
        stockCount.text = "" + item.stock+"x";
        stockCount.gameObject.SetActive(item.stock > 1);
    }

    public void Clicked()
    {
        Debug.Log("ItemClicked!" + item.name);
        FindObjectOfType<UIMerchantController>().ItemClicked(item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }
}
