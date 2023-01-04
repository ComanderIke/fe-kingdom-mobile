using UnityEngine;

namespace LostGrace
{
    public class BottomUIBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
   

        public void Show()
        {
            gameObject.SetActive(true);
            TweenUtility.FadeIn(canvasGroup);
            
        }

        public virtual void Hide()
        {
            TweenUtility.FadeOut(canvasGroup).setOnComplete(()=>gameObject.SetActive(false));
        }
    }
}