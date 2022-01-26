using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIMerchantController : MonoBehaviour
{
    private MerchantEncounterNode node;
    public Canvas canvas;
    [HideInInspector]
    public Party party;
    // public TextMeshProUGUI personName;
    //public TextMeshProUGUI talkText;
    public List<UIShopItemController> shopItems;
    private Merchant merchant;
    public void Show(MerchantEncounterNode node, Party party)
    {
        canvas.enabled = true;
        this.node = node;
        this.party = party;
        this.merchant = node.merchant;
        for (int i=0; i<merchant.shopItems.Count; i++)
        {
            var item = merchant.shopItems[i];
            shopItems[i].SetValues(new ShopItem(item.cost, item.Sprite, item.Description));
        }
        FindObjectOfType<UICharacterViewController>().Show(party);
    }
    public void ContinueClicked()
    {
        canvas.enabled=false;
        node.Continue();
        FindObjectOfType<UICharacterViewController>().Hide();
    }
}
