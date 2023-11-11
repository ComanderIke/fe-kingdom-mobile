using UnityEngine;

namespace LostGrace
{
    public class BottomUIBase : MonoBehaviour
    {
        [SerializeField] public CanvasGroup canvasGroup;
        [SerializeField] public CanvasGroup parentCanvasGroup;
        [SerializeField] public float inActiveAlpha = .7f;

        public void Show()
        {
            LeanTween.cancel(canvasGroup.gameObject);
            gameObject.SetActive(true);
            TweenUtility.FadeIn(canvasGroup);
            
        }

        public virtual void Hide()
        {
            LeanTween.cancel(canvasGroup.gameObject);
            TweenUtility.FadeOut(canvasGroup).setOnComplete(()=>gameObject.SetActive(false));
        }
    }
}