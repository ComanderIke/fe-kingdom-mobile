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

    public void Show(SmithyEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.smithy = node.smithy;
        for (int i=0; i<smithy.shopItems.Count; i++)
        {
            var item = smithy.shopItems[i];
            bool affordable = party.money >= item.cost;
            shopItems[i].SetValues(new ShopItem(item.name, item.cost, item.Sprite, item.Description), affordable);
        }
        FindObjectOfType<UICharacterViewController>().Show(party);
    }

    public void ContinueClicked()
    {
        FindObjectOfType<UICharacterViewController>().Hide();
        canvas.enabled = false;
        node.Continue();
    }
}
