using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UILoopPingPongFade : MonoBehaviour
{
    public float maxAlpha= 1;
    public float minAlpha = 0;
    public float duration = 0.8f;
    // Start is called before the first frame update

    public void StartAnimation()
    {
        LeanTween.cancel(this.gameObject);
        GetComponent<CanvasGroup>().alpha = minAlpha;
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), maxAlpha, duration).setLoopPingPong();
    }

}
