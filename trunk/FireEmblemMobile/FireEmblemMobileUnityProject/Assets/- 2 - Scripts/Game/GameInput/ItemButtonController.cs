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
        public IItemClickedReceiver receiver;
        public void SetItem(StockedItem item)
        {
            this.item = item;
            UpdateValues();
        }
        private void UpdateValues()
        {
            Icon.sprite = item.item.Sprite;
            text.text = item.item.Name;
        }

        public void Clicked()
        {
            receiver.ItemClicked(item);
            new GameplayInput().SelectItem(item.item);
        }
    }
}