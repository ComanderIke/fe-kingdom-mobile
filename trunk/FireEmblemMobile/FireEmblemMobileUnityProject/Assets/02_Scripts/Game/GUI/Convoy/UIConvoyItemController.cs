using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConvoyItemController : UIButtonController
{
    private StockedItem item;
    public TextMeshProUGUI stockCount;
    [SerializeField] private Image gemImage;
    [SerializeField] private GameObject slot;
    public void SetValues(StockedItem stockeditem, bool grayedOut=false)
    {
        if (stockeditem.item is Relic relic)
        {
           slot.gameObject.SetActive(relic.slotCount>0);
           if (relic.GetGem(0) == null)
           {
               gemImage.enabled = false;
               gemImage.sprite = null;
           }
           else
           {
               gemImage.enabled = true;
               gemImage.sprite = relic.GetGem(0).Sprite;
           }
        }
        else
        {
            slot.gameObject.SetActive(false);
        }
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