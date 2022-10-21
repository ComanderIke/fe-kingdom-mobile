using UnityEngine;

public class TweenUtility
{
    private const float FadeInDuration = 0.65f;
    private const float FadeInDurationFast = 0.25f;
    private const float FadeOutDuration = 0.4f;
    private const LeanTweenType easeFadeIn = LeanTweenType.easeOutQuad;
    private const LeanTweenType easeFadeOut =  LeanTweenType.easeInQuad;

    public static LTDescr FadeIn(CanvasGroup a)
    {
        return LeanTween.alphaCanvas(a, 1, FadeInDuration).setEase(easeFadeIn);
    }
    public static LTDescr FadeOut(CanvasGroup a)
    {
        return LeanTween.alphaCanvas(a, 0, FadeOutDuration).setEase(easeFadeOut);
    }

    public static LTDescr FastFadeIn(CanvasGroup a)
    {
        return LeanTween.alphaCanvas(a, 1, FadeInDurationFast).setEase(easeFadeIn);
    }
}