using UnityEngine;

namespace LostGrace
{
    public class BottomUIBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
   

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