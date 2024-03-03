using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class UISoldOutArea : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI bigRedText;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private CanvasGroup canvasGroup;
        public void SetStateSoldOut()
        {
            bigRedText.text = "Sold out";
            description.text = "Nothing left to buy for now";
            canvasGroup.alpha = 1f;
        }
        public void SetStateNothingToSell()
        {
            bigRedText.text = "Empty";
            description.text = "You have nothing to sell.";
            canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            canvasGroup.alpha = .0f;
        }
    }
}
