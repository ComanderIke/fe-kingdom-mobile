using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IShopItemClickedReceiver
{
    void ItemClicked(UIConvoyItemController item);
}
public class UIConvoyItemController : UIButtonController
{
    [HideInInspector]public StockedItem stockedItem;
    public TextMeshProUGUI stockCount;
    [SerializeField] private Image gemImage;
    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject showTooltipButtonArea;
    public event Action<UIConvoyItemController> onClicked;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite relicSprite;
    [SerializeField] private Sprite combatItemSprite;
    [SerializeField] private MMF_Player addedFeedbacks;
    [HideInInspector] public int index;
    private IShopItemClickedReceiver clickedReceiver;
    [SerializeField] private float tooExpensiveAlpha = 1f;
    private bool affordable = false;
    public Color tooExpensiveColor;
    public Color normalColor;
    public TextMeshProUGUI cost;
    public Image costIcon;
    public void SetValues(StockedItem stockeditem,bool affordable, bool first,IShopItemClickedReceiver receiver, bool showTooltipButton)
    {
       
        ShopItemState();
        this.affordable = affordable;
        this.clickedReceiver = receiver;
        this.cost.SetText(""+stockeditem.item.cost);
        showTooltipButtonArea.gameObject.SetActive(showTooltipButtonArea);
        if (affordable)
        {
            cost.color = normalColor;
            canvasGroup.alpha = 1;
        }
        else
        {
            cost.color = tooExpensiveColor;
            canvasGroup.alpha = tooExpensiveAlpha;
        }
        UpdateUI(stockeditem,  first);
        
    }

    void ShopItemState()
    {
        costIcon.gameObject.SetActive(true);
        cost.gameObject.SetActive(true);
    }
    void ConvoyItemState()
    {
        costIcon.gameObject.SetActive(false);
        cost.gameObject.SetActive(false);
    }

    private void UpdateUI(StockedItem stockeditem,bool added=false,bool grayedOut=false)
    {
        if(added)
            addedFeedbacks.PlayFeedbacks();
        backgroundImage.sprite = normalSprite;
        this.index = index;
        if (stockeditem.item is Relic relic)
        {
            backgroundImage.sprite = relicSprite;
            slot.gameObject.SetActive(true);
            if (relic.GetGem() == null)
            {
                gemImage.enabled = false;
                gemImage.sprite = null;
            }
            else
            {
                gemImage.enabled = true;
                gemImage.sprite = relic.GetGem().Sprite;
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
        SetValues(stockeditem.item.Sprite, grayedOut);
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
    public void SetValues(StockedItem stockeditem, int index, bool added=false,bool grayedOut=false, bool showTooltipButton=true)
    {
        MyDebug.LogTest("CONVOYSTATE WHUT?");
        showTooltipButtonArea.gameObject.SetActive(showTooltipButton);
        ConvoyItemState();
        UpdateUI(stockeditem,added,grayedOut);
       
        
       
        
    }

    public void TooltipClicked()
    {
        ToolTipSystem.Show(stockedItem, transform.position);
    }

    public virtual void Clicked()
    {
        onClicked?.Invoke(this);
        clickedReceiver?.ItemClicked(this);
       
        //FindObjectOfType<UIMerchantController>().ItemClicked(item.item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }
}