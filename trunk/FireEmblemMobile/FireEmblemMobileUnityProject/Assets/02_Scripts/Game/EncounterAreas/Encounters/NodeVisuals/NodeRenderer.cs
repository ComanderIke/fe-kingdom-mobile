using UnityEngine;

public class NodeRenderer : MonoBehaviour
{
    [SerializeField] GameObject moveOptionPrefab;
    private GameObject moveEffect;
    public void MovableAnimation()
    {
        moveEffect = Instantiate(moveOptionPrefab, transform, false);
        Vector3 scale = gameObject.transform.localScale;
        LeanTween.scale(gameObject, scale*1.07f,0.8f).setLoopType(LeanTweenType.pingPong);
    }

    public void Reset()
    {
        LeanTween.cancel(gameObject);
        Destroy(moveEffect);
    }

    public void GrowAnimation()
    { 
        Debug.Log("Show Grow Animation");
        if (moveEffect != null)
        {
            Debug.Log("MoveEffect not null");
          
            foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
            {
                
                Debug.Log("Grab Shape");
               
                LeanTween.value(0, 1, .7f).setEasePunch().setOnUpdate((value) =>
                {
                    ps.transform.localScale = Vector3.one* (1 +value*0.2f);
                });
            }
        }
    }

    
}