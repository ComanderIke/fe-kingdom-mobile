using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UISmithyController : MonoBehaviour
{
    public Canvas canvas;
    public SmithyEncounterNode node;
    [HideInInspector]
    public Party party;
    public List<UIShopItemController> shopItems;
    private Smithy smithy;

    public void Show(SmithyEncounterNode node)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = node.party;
        this.smithy = node.smithy;
        for (int i=0; i<smithy.shopItems.Count; i++)
        {
            var item = smithy.shopItems[i];
            shopItems[i].SetValues(new ShopItem(item.cost, item.Sprite, item.Description));
        }
    }

    public void ContinueClicked()
    {
        canvas.enabled = false;
        node.Continue();
    }
}
