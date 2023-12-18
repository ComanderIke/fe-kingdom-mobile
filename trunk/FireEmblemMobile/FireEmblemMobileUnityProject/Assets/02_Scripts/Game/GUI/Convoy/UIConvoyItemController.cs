using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConvoyItemController : UIButtonController
{
    public StockedItem stockedItem;
    public TextMeshProUGUI stockCount;
    [SerializeField] private Image gemImage;
    [SerializeField] private GameObject slot;
    public event Action<UIConvoyItemController> onClicked;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite relicSprite;
    [SerializeField] private Sprite combatItemSprite;
    [SerializeField] private MMF_Player addedFeedbacks;
    public int index;
    public void SetValues(StockedItem stockeditem, int index, bool added=false,bool grayedOut=false)
    {
        if(added)
            addedFeedbacks.PlayFeedbacks();
        backgroundImage.sprite = normalSprite;
        this.index = index;
        if (stockeditem.item is Relic relic)
        {
            backgroundImage.sprite = relicSprite;
           slot.gameObject.SetActive(true);
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
        else if (stockeditem.item is IEquipableCombatItem)
        {
            backgroundImage.sprite = combatItemSprite;
            slot.gameObject.SetActive(false);
        }
        else
        {
            slot.gameObject.SetActive(false);
        }
        this.stockedItem = stockeditem;
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
        onClicked?.Invoke(this);
      
       
        //FindObjectOfType<UIMerchantController>().ItemClicked(item.item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }
}