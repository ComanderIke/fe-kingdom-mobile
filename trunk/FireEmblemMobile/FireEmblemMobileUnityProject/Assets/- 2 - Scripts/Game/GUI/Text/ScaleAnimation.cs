using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (!LeanTween.isTweening(gameObject.GetComponent<RectTransform>()))
        {
            gameObject.transform.localScale = Vector3.one;
            LeanTween.scale(gameObject.GetComponent<RectTransform>(), Vector3.one * 1.05f, 0.5f).setLoopPingPong();
        }
        else
        {
            gameObject.transform.localScale = Vector3.one;
            LeanTween.resume(gameObject);
        }
    }

    void OnDisable()
    {
        LeanTween.pause(gameObject);
    }
}
