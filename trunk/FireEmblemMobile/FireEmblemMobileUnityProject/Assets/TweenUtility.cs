using UnityEngine;

namespace LostGrace
{
    public class TweenUtility
    {
        private const float fadeInTime= .6f;
        private const float slowfadeInTime= 1.6f;
        private const float fadeOutTime = .35f;
        public static LTDescr FadeIn(CanvasGroup canvasGroup)
        {
            return LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setEaseOutQuad();
        }
        public static LTDescr FadeOut(CanvasGroup canvasGroup)
        {
            return LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setEaseInQuad();
        }

        public static LTDescr SlowFadeIn(CanvasGroup canvasGroup)
        {
            return LeanTween.alphaCanvas(canvasGroup, 1, slowfadeInTime).setEaseOutQuad();
        }
    }
}