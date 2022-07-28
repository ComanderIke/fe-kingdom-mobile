using UnityEngine;

public class NodeRenderer : MonoBehaviour
{
    
    public void MovableAnimation()
    {
        Vector3 scale = gameObject.transform.localScale;
        LeanTween.scale(gameObject, scale*1.07f,0.8f).setLoopType(LeanTweenType.pingPong);
    }

    public void Reset()
    {
        LeanTween.cancel(gameObject);
    }
}