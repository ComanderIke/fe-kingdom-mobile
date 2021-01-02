using Assets.Utility;
using UnityEngine;

public class UIWhiteBlinkAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration=1;
    void OnEnable()
    {
        float start = FindObjectOfType<AnimationTimer>().NormalizedTime;
        LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0.2f, duration - (duration * start)).setOnComplete(()=>LeanTween.alphaCanvas(GetComponent<CanvasGroup>(), 0f, duration).setLoopPingPong());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
