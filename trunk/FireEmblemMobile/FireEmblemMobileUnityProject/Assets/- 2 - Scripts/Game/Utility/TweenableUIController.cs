using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Utility
{
    public abstract class TweenableUIController : MonoBehaviour
    {
        // Start is called before the first frame update
        public RectTransform tweenObj;
        public UnityEvent AfterHide;
        public CanvasGroup CanvasGroup;

        public void Hide()
        {

            LeanTween.scale(tweenObj, Vector3.zero, 0.4f).setDelay(0.0f).setEaseInQuad()
                .setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    AfterHide?.Invoke();
                    
                });
            LeanTween.alphaCanvas(CanvasGroup, 0, 0.4f).setDelay(0.0f).setEaseInQuad()
                .setOnComplete(() =>
                {
                    // gameObject.SetActive(false);
                    // AfterHide?.Invoke();
                    
                });
        }
        public void Show()
        {
            gameObject.SetActive(true);
            tweenObj.transform.localScale = Vector3.zero;
            LeanTween.scale(tweenObj, Vector3.one, 0.6f).setDelay(1.0f).setEase( LeanTweenType.easeOutBack ).setOvershoot(0.5f);//.setPeriod(0.1f);
            CanvasGroup.alpha = 0;
            LeanTween.alphaCanvas(CanvasGroup, 1, 0.6f).setDelay(1.0f).setEaseInQuad();
        }
    }
}
