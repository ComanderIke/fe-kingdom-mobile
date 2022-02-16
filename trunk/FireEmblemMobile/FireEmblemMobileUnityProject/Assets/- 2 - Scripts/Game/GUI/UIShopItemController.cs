using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopItemController : UIButtonController
{
    public Color tooExpensiveColor;
    public Color normalColor;
    private ShopItem item;
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
    }

    public void Clicked()
    {
        Debug.Log("ItemClicked!" + item.name);
        FindObjectOfType<UIMerchantController>().ItemClicked(item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }
}
