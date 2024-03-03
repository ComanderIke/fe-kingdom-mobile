using UnityEngine;

namespace Game.GUI.Other
{
    public class UILoopPingPongFade : MonoBehaviour
    {
        public float maxAlpha= 1;
        public float minAlpha = 0;
        public float duration = 0.8f;
        private CanvasGroup CanvasGroup;
        // Start is called before the first frame update

        public void OnEnable()
        {
            if (CanvasGroup == null)
                CanvasGroup = GetComponent<CanvasGroup>();
            if (LeanTween.isTweening(gameObject))
            {
                CanvasGroup.alpha = minAlpha;
                LeanTween.resume(gameObject);
            }
            else
            {
                CanvasGroup.alpha = minAlpha;
                LeanTween.alphaCanvas(CanvasGroup, maxAlpha, duration).setLoopPingPong();
            }
        }
        private void OnDisable()
        {
            LeanTween.pause(gameObject);
        }
    }
}
