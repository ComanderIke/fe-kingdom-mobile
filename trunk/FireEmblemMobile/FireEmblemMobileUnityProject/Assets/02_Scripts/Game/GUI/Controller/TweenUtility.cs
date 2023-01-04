using UnityEngine;

public class TweenUtility
{
    public const float fadeInDuration = 0.65f;
    private const float fastFadeInDuration = 0.25f;
    public const float fadeOutDuration = 0.4f;
    private const float slowFadeInDuration = 1.6f;
    public const LeanTweenType easeFadeIn = LeanTweenType.easeOutQuad;
    public const LeanTweenType easeFadeOut =  LeanTweenType.easeInQuad;

    public static LTDescr FadeIn(CanvasGroup a)
    {
        return LeanTween.alphaCanvas(a, 1, fadeInDuration).setEase(easeFadeIn);
    }
    public static LTDescr FadeOut(CanvasGroup a)
    {
        return LeanTween.alphaCanvas(a, 0, fadeOutDuration).setEase(easeFadeOut);
    }

    public static LTDescr FastFadeIn(CanvasGroup a)
    {
        return LeanTween.alphaCanvas(a, 1, fastFadeInDuration).setEase(easeFadeIn);
    }
    public static LTDescr SlowFadeIn(CanvasGroup canvasGroup)
    {
        return LeanTween.alphaCanvas(canvasGroup, 1, slowFadeInDuration).setEaseOutQuad();
    }
}