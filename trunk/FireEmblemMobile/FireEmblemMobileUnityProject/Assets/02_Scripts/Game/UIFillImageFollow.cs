using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIFillImageFollow : MonoBehaviour
    {
        [SerializeField]private Image fillImage;
        private RectTransform rectTransform;
        void Start()
        {
            rectTransform = transform as RectTransform;
        }

        void Update()
        {
            float fillAmount = fillImage.fillAmount;
            float startPosX = (fillImage.rectTransform.rect.center.x - fillImage.rectTransform.rect.width / 2);
            float startPosY = fillImage.rectTransform.rect.center.y;
            Vector2 startPos = new Vector2(startPosX, startPosY);
            float fillWidth = fillImage.rectTransform.rect.width * fillAmount;
            Vector2 goalPos = new Vector2(startPos.x + fillWidth, startPos.y);
            rectTransform.anchoredPosition = goalPos;

        }
    }
}
