using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIConvoyItemController : UIButtonController
{
    private StockedItem item;
    public TextMeshProUGUI stockCount;
    public void SetValues(StockedItem stockeditem, bool grayedOut=false)
    {
        this.item = stockeditem;
        SetValues(stockeditem.item.Sprite, stockeditem.item.Description, grayedOut);
        stockCount.text = "" + stockeditem.stock+"x";
        stockCount.gameObject.SetActive(stockeditem.stock > 1);
        if (grayedOut)
        {
            canvasGroup.alpha = .6f;
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
            canvasGroup.alpha = 1.0f;
        }
    }

    public void Clicked()
    {
        Debug.Log("ItemClicked!" + item.item.Name);
        ToolTipSystem.Show(item.item, transform.position, item.item.Name, item.item.Description, item.item.Sprite);
        //FindObjectOfType<UIMerchantController>().ItemClicked(item.item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }
}