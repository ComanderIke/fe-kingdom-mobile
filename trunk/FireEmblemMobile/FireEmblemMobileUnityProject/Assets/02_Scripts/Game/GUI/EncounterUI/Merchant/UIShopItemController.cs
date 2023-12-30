using System;
using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IShopItemClickedReceiver
{
    void ItemClicked(StockedItem item);
}
public class UIShopItemController : UIButtonController
{
    public Color tooExpensiveColor;
    public Color normalColor;
    public TextMeshProUGUI cost;
    public StockedItem item;
    public TextMeshProUGUI stockCount;
    private IShopItemClickedReceiver clickedReceiver;
    [SerializeField] private float tooExpensiveAlpha = 1f;
  
    private bool affordable = false;
    public void SetValues(StockedItem item, bool affordable,IShopItemClickedReceiver receiver)
    {
        this.cost.SetText(""+item.item.cost);
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
            image.sprite = item.item.Sprite;
            stockCount.text = "" + item.stock + "x";
            stockCount.gameObject.SetActive(item.stock > 1);


            base.UpdateUI();
        }

    }

  
    public void Clicked()
    {
        Debug.Log("ItemClicked!" + item.item.Name);
        clickedReceiver.ItemClicked(item);
        // ToolTipSystem.Show(transform.position, item.name, item.description, item.sprite);
    }

    public void SetAffordable(bool canAfford)
    {
        this.affordable = canAfford;
        UpdateUI();
    }
}
