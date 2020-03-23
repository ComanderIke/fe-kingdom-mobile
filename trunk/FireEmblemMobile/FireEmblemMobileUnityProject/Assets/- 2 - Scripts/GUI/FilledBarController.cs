using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class FilledBarController : MonoBehaviour
    {
        [SerializeField] private Image[] barCurrentImages = default;
        [SerializeField] private Image[] barColoredImages = default;

        public void SetFillAmount(float fillAmount)
        {
            foreach (var img in barCurrentImages)
            {
                img.fillAmount = fillAmount;
            }
        }
        public void SetColor(Color color)
        {
            foreach (var img in barColoredImages)
            {
                img.color = color;
            }
        }

    }
}
