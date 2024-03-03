using System;
using Game.GameActors.Items.Gems;
using Game.GUI.Buttons;
using Game.GUI.EncounterUI.Merchant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Screens
{
    public class SelectableItemController : UIButtonController
    {
        [HideInInspector]
        public StockedItem item;

        [SerializeField] private Image sockelIcon;
        public TextMeshProUGUI stockCount;
        public event Action<SelectableItemController> onClicked;
    
        public void SetValues(StockedItem item)
        {
            this.item = item;
            image.sprite = item.item.Sprite;
      
            UpdateUI();
        }

        protected override void UpdateUI()
        {
            sockelIcon.enabled = false;
            if (item.item is Gem gem)
            {
                if (gem.IsInserted())
                {
                    sockelIcon.enabled = true;
                }
            }
            stockCount.text = "" + item.stock + "x";
            stockCount.gameObject.SetActive(item.stock > 1);
            base.UpdateUI();
        }


        public void Clicked()
        {
            onClicked?.Invoke(this);
        }

        public StockedItem GetItem()
        {
            return item;
        }
    }
}