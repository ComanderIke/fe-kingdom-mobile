using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class UIChurchController : MonoBehaviour
{
    public Canvas canvas;
    public ChurchEncounterNode node;
    [HideInInspector]
    public Party party;
    public List<UIShopItemController> shopItems;
    private Church church;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show(ChurchEncounterNode node)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = node.party;
        this.church = node.church;
        for (int i=0; i<church.shopItems.Count; i++)
        {
            var item = church.shopItems[i];
            shopItems[i].SetValues(new ShopItem(item.cost, item.Sprite, item.Description));
        }
    }

    public void ContinueClicked()
    {
        canvas.enabled = false;
        node.Continue();
    }
}
