using System;
using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IShopItemClickedReceiver
{
    void ItemClicked(ShopItem item);
}
public class UIShopItemController : UIButtonController
{
    public Color tooExpensiveColor;
    public Color normalColor;
    public TextMeshProUGUI cost;
    public ShopItem item;
    public TextMeshProUGUI stockCount;
    private IShopItemClickedReceiver clickedReceiver;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float tooExpensiveAlpha = 0.6f;
  
    private bool affordable = false;
    public void SetValues(ShopItem item, bool affordable,IShopItemClickedReceiver receiver)
    {
        this.cost.SetText(""+item.cost);
        this.item = item;
        this.clickedReceiver = receiver;
        this.affordable = affordable;
        UpdateUI();
    }

    protected override void UpdateUI()
    {
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


        if (item != null)
        {
            stockCount.text = "" + item.stock + "x";
            stockCount.gameObject.SetActive(item.stock > 1);


            base.UpdateUI();
        }

    }

  
    public void Clicked()
    {
        Debug.Log("ItemClicked!" + item.name);
        clickedReceiver.ItemClicked(item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }

    public void SetAffordable(bool canAfford)
    {
        this.affordable = canAfford;
        UpdateUI();
    }
}
