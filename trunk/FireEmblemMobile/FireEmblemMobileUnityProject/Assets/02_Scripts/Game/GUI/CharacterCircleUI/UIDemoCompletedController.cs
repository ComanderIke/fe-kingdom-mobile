using TMPro;
using UnityEngine;

namespace LostGrace
{
    public class UIDemoCompletedController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInTime=1.2f;
        [SerializeField] private float stayDuration=2f;
        [SerializeField] private float fadeOutTime=1f;
        // Start is called before the first frame update
        void Start()
        {
            AreaGameManager.OnDemoCompleted -= Show;
            AreaGameManager.OnDemoCompleted += Show;
        }

        private void OnDestroy()
        {
            AreaGameManager.OnDemoCompleted -= Show;
        }

        // Update is called once per frame
        void Show()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setEaseInOutQuad().setOnComplete(() =>
                LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setEaseInOutQuad().setDelay(stayDuration));
        }
    }
}