using Game.GUI.EncounterUI.Merchant;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameInput.Buttons
{
    public class ItemButtonController:MonoBehaviour
    {
        private StockedItem item;
        public Image Icon;
        public TextMeshProUGUI text;
        public SelectionUI selectionUI;
        public void SetItem(StockedItem item, SelectionUI selectionUI)
        {
            this.item = item;
            UpdateValues();
            this.selectionUI = selectionUI;
        }
        private void UpdateValues()
        {
            Icon.sprite = item.item.Sprite;
            text.text = item.item.Name;
        }

        public void Clicked()
        {
            Debug.Log("GameplayCommandSelectItem");
            new GameplayCommands.GameplayCommands().SelectItem(item.item);
            //selectionUI.CloseItemsClicked();
        }
    }
}