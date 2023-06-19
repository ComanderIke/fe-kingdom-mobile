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

    public void SetValues(Relic relic, string header, string description, Sprite icon, Vector3 position)
    {
        this.relic = relic;
        Debug.Log(""+relic.GetSlotCount());
        if (relic.GetSlotCount() != 0)
        {
            
            slotButtonPanel.gameObject.SetActive(true);
            if (relic.GetGem(0) != null)
            {
                gemImage.sprite = relic.GetGem(0).Sprite;
                gemImage.gameObject.SetActive(true);
            }
            else
            {
                gemImage.gameObject.SetActive(false);
            }
        }
        else
        {
            slotButtonPanel.gameObject.SetActive(false);
           
        }
        base.SetValues(relic, header, description, icon, position);
    }

    public void SlotClicked()
    {
        convoeyUI.ShowGemOptions();
        Debug.Log("Open ItemPanel with all gemstones and a none option!");
        
    }
}
