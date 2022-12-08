using UnityEngine;

namespace Game.GUI.Text
{
    
    public class ScaleAnimation : MonoBehaviour
    {
        // Start is called before the first frame update
        void OnEnable()
        {
            var rectT = gameObject.GetComponent<RectTransform>();
            if (!LeanTween.isTweening(rectT))
            {
                gameObject.transform.localScale = Vector3.one;
                LeanTween.scale(rectT, Vector3.one * 1.05f, 0.5f).setLoopPingPong();
               
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
}
