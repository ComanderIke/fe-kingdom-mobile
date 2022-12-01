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
    public Color normalBgColor;
    public Color selectedBgColor;
    [SerializeField] private Image backGround;
    public ShopItem item;
    public TextMeshProUGUI stockCount;
    private IShopItemClickedReceiver clickedReceiver;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float tooExpensiveAlpha = 0.6f;
    private bool selected = false;
    private bool affordable = false;
    public void SetValues(ShopItem item, bool affordable,IShopItemClickedReceiver receiver)
    {
        this.item = item;
        this.clickedReceiver = receiver;
        this.affordable = affordable;
        UpdateUI();
    }

    private void UpdateUI()
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
            SetValues(item.sprite, item.cost, item.description);
            stockCount.text = "" + item.stock + "x";
            stockCount.gameObject.SetActive(item.stock > 1);
        }

        backGround.color = selected ? selectedBgColor : normalBgColor;
    }

    public void Select()
    {
        selected = true;
        UpdateUI();
    }
    public void Deselect()
    {
        selected = false;
        UpdateUI();
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
