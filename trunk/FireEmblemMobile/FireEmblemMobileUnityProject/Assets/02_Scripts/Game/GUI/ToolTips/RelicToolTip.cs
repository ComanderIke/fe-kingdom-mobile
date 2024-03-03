using Game.GameActors.Items.Weapons;
using Game.GUI.Convoy;
using Game.GUI.EncounterUI.Merchant;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.ToolTips
{
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
}
