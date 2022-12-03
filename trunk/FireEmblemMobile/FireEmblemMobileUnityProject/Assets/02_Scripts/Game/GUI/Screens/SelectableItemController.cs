using System;
using Game.GameActors.Items;
using TMPro;
using UnityEngine;

public class SelectableItemController : UIButtonController
{
    [HideInInspector]
    public StockedItem item;
    public TextMeshProUGUI stockCount;
    public event Action<SelectableItemController> onClicked;
    
    public void SetValues(StockedItem item)
    {
        this.item = item;
        UpdateUI();
    }

    protected override void UpdateUI()
    {

        stockCount.text = "" + item.stock + "x";
        stockCount.gameObject.SetActive(item.stock > 1);
        base.UpdateUI();
    }


    public void Clicked()
    {
        onClicked?.Invoke(this);
    }

    public StockedItem GetItem()
    {
        return item;
    }
}