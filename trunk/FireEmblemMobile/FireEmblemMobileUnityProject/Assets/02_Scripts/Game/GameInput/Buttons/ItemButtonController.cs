using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameInput
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
            new GameplayCommands().SelectItem(item.item);
            //selectionUI.CloseItemsClicked();
        }
    }
}