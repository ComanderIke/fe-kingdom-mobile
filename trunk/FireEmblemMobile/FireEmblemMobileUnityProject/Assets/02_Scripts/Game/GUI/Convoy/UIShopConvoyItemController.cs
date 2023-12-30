using TMPro;
using UnityEngine;

public class UIShopConvoyItemController : UIConvoyItemController
{
    private IShopItemClickedReceiver clickReceiver;
    [SerializeField]private TextMeshProUGUI itemCost;
    [SerializeField]private Color tooExpensiveTextColor;
    [SerializeField] private Color normalTextColor;
    [SerializeField] private float tooExpensiveAlpha=.6f;
    
    public void SetValues(StockedItem stockeditem, int index, bool affordable, IShopItemClickedReceiver clickReceiver)
    {
        this.clickReceiver = clickReceiver;
        base.SetValues(stockeditem,index, true, false);
        if (affordable)
        {
            itemCost.color = normalTextColor;
            canvasGroup.alpha = 1;
        }
        else
        {
            itemCost.color = tooExpensiveTextColor;
            canvasGroup.alpha = tooExpensiveAlpha;
        }

    }

    public override void Clicked()
    {
        base.Clicked();
        clickReceiver.ItemClicked(stockedItem);
    }
}