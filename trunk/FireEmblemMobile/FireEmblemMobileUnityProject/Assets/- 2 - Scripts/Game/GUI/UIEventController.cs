using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIEventController : MonoBehaviour
{
    public Canvas canvas;
    public EventEncounterNode node;
    [HideInInspector]
    public Party party;
    private RandomEvent randomEvent;
    // Start is called before the first frame update
    public void Show(EventEncounterNode node)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = node.party;
        this.randomEvent = node.randomEvent;
        // for (int i=0; i<church.shopItems.Count; i++)
        // {
        //     var item = church.shopItems[i];
        //     shopItems[i].SetValues(new ShopItem(item.cost, item.Sprite, item.Description));
        // }
    }
    public void ContinueClicked()
    {
        canvas.enabled = false;
        node.Continue();
    }
}
