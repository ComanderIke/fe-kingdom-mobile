using System.Collections.Generic;
using Game.GameActors.Items;
using UnityEngine;

[System.Serializable]
public class ShopItemBp
{
    public ItemBP item;
    public int stock;
}
[CreateAssetMenu(menuName = "GameData/Merchant", fileName = "Merchant")]
public class MerchantBP:ScriptableObject
{
    public List<ShopItemBp> items;
    [SerializeField] private Sprite merchantFace;
    [SerializeField] private string merchantName;
    public Merchant Create()
    {
        var merchant = new Merchant(merchantFace, merchantName);
        foreach (var item in items)
        {
            merchant.AddItem(new ShopItem(item.item.Create(), item.stock));
        }

        return merchant;
    }
}