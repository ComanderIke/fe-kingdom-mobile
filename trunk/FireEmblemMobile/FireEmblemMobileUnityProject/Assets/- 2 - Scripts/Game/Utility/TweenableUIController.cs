using UnityEngine;

namespace Game.Utility
{
    public abstract class TweenableUIController : MonoBehaviour
    {
        // Start is called before the first frame update
        public RectTransform tweenObj;

        public void Hide()
        {
            
            LeanTween.scale(tweenObj, Vector3.zero, 0.8f).setDelay(1.0f).setEaseInOutQuad().setOnComplete(()=>gameObject.SetActive(false));
        }
        public void Show()
        {
            gameObject.SetActive(true);
            tweenObj.transform.localScale = Vector3.zero;
            LeanTween.scale(tweenObj, Vector3.one, 0.8f).setDelay(1.0f).setEaseInOutQuad();
        }
    }
}
