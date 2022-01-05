using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopItemController : UIButtonController
{
    public void SetValues(ShopItem item)
    {
        SetValues(item.sprite, item.cost, item.description);
    }
}
