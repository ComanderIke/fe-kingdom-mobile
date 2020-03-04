using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class FilledBarController : MonoBehaviour
    {
        [SerializeField] private Image[] barCurrentImages = default;

        public void SetFillAmount(float fillAmount)
        {
            foreach (var img in barCurrentImages)
            {
                img.fillAmount = fillAmount;
            }
        }
    }
}
