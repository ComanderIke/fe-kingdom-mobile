using UnityEngine;

public class TweenUtility
{
    private const float FadeInDuration = 0.65f;
    private const float FadeInDurationFast = 0.25f;
    private const float FadeOutDuration = 0.4f;
    private const LeanTweenType easeFadeIn = LeanTweenType.easeOutQuad;
    private const LeanTweenType easeFadeOut =  LeanTweenType.easeInQuad;

    public static void FadeIn(CanvasGroup a)
    {
        LeanTween.alphaCanvas(a, 1, FadeInDuration).setEase(easeFadeIn);
    }
    public static void FadeOut(CanvasGroup a)
    {
        LeanTween.alphaCanvas(a, 0, FadeOutDuration).setEase(easeFadeOut);
    }

    public static void FastFadeIn(CanvasGroup a)
    {
        LeanTween.alphaCanvas(a, 1, FadeInDurationFast).setEase(easeFadeIn);
    }
}