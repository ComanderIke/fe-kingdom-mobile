using __2___Scripts.Game.Utility;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameInput;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.UI;

public class RelicToolTip : ItemToolTip
{
    [SerializeField] private GameObject slotButtonPanel;
    [SerializeField] private Button slotButton;
    [SerializeField] private UIConvoyController convoeyUI;
    [SerializeField] private Image gemImage;
    private Relic relic;

    public void SetValues(Relic relic,  Vector3 position)
    {
        this.relic = relic;
        slotButtonPanel.gameObject.SetActive(true);
        if (relic.GetGem() != null)
        {
            gemImage.sprite = relic.GetGem().Sprite;
            gemImage.gameObject.SetActive(true);
        }
        else
        {
            gemImage.gameObject.SetActive(false);
        }
       
        base.SetValues(new StockedItem(relic,1), position);
    }

    public void SlotClicked()
    {
        convoeyUI.ShowGemOptions();
        Debug.Log("Open ItemPanel with all gemstones and a none option!");
        
    }
}
