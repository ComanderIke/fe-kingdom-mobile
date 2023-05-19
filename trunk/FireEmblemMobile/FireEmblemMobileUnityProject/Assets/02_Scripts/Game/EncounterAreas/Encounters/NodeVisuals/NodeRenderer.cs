using System;
using UnityEngine;

public class NodeRenderer : MonoBehaviour
{
    [SerializeField] GameObject moveOptionPrefab;
    private GameObject moveEffect;
    [SerializeField] public Sprite moveOptionSprite;
    [SerializeField] public Color typeColor;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private SpriteRenderer nodeCircleRenderer;


    

    private const float defaultRotationSpeed = 25;
    const float defaultScale = 1.5f;
    readonly  Vector3 nodeStartScale = new Vector3(0.12f,0.12f,0.12f);

    
    public void MovableAnimation()
    {
        moveEffect = Instantiate(moveOptionPrefab, transform, false);
        foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
        {
            ps.transform.localScale = new Vector3(defaultScale, defaultScale, defaultScale);
        }
 
        LeanTween.scale(gameObject, nodeStartScale*1.07f,0.8f).setLoopType(LeanTweenType.pingPong);
    }

    public void Awake()
    {
      
        gameObject.transform.localScale = nodeStartScale;
    }
    public void Reset()
    {
      
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, nodeStartScale,0.2f).setEaseInQuad();
        
       // Debug.Log("Reset to "+gameObject.transform.localScale.x);
       Destroy(moveEffect);
    }

    public void SetInactive()
    {
        LeanTween.color(nodeCircleRenderer.gameObject, inactiveColor, .5f).setEaseInQuad();
        LeanTween.color(iconRenderer.gameObject, inactiveColor, .5f).setEaseInQuad();
    }

    public void GrowAnimation()
    {
        if (moveEffect != null)
        {

            foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
            {
                
               
                LeanTween.value(0, 1, .7f).setEasePunch().setOnUpdate((value) =>
                {
                    var newScale = defaultScale + value * 0.2f;
                    ps.transform.localScale = new Vector3(newScale, newScale, newScale);
                });
            }
        }
    }


    public void IncreaseScale(float scale)
    {
        if (moveEffect != null)
        {
        
          
            foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
            {
                ps.transform.localScale = new Vector3(defaultScale+scale,defaultScale+scale,defaultScale+scale);
            }
        }
    }

    public void AmplifyRotationSpeedMultiplier(float orbitalRotationSpeed)
    {
        if (moveEffect != null)
        {

            foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
            {
                var velomodule = ps.velocityOverLifetime;
                velomodule.orbitalZ= defaultRotationSpeed + orbitalRotationSpeed;
               
            }
        }
    }
}