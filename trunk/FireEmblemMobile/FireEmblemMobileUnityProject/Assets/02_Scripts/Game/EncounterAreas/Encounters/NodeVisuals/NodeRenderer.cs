using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeRenderer : MonoBehaviour
{
    [SerializeField] GameObject moveOptionPrefab;
    private GameObject moveEffect;
    [SerializeField] public Sprite moveOptionSprite;
    [SerializeField] private float inactiveAlpha;
   // [SerializeField] private SpriteRenderer iconRenderer;
    //[SerializeField] private SpriteRenderer nodeCircleRenderer;
    [SerializeField] private List<SpriteRenderer> fadeAffected;

    [SerializeField] private bool big;

    private const float defaultRotationSpeed = 25;
    const float defaultScale = 1.5f;
    const float bigScale = 2.0f;

    readonly  Vector3 nodeStartScale = new Vector3(0.12f,0.12f,0.12f);
    readonly  Vector3 nodeBigStartScale = new Vector3(0.2f,0.2f,0.2f);

    
    public void MovableAnimation()
    {
        moveEffect = Instantiate(moveOptionPrefab, transform, false);
        Vector3 particleScale= big?new Vector3(bigScale, bigScale, bigScale):new Vector3(defaultScale, defaultScale, defaultScale);
        foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
        {
            if (big)
                ps.transform.localScale = particleScale;
        }
 
        LeanTween.scale(gameObject, (big?nodeBigStartScale:nodeStartScale)*1.07f,0.8f).setLoopType(LeanTweenType.pingPong);
    }

    public void Awake()
    {
      
        gameObject.transform.localScale =big?nodeBigStartScale: nodeStartScale;
        foreach (var spriteRenderer in fadeAffected)
        {
            spriteRenderer.color = new Color(1,1,1, inactiveAlpha);

        }
    }
    public void Reset()
    {
      
        
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, big?nodeBigStartScale:nodeStartScale,0.2f).setEaseInQuad();
        
       // Debug.Log("Reset to "+gameObject.transform.localScale.x);
       Destroy(moveEffect);
    }

    public void SetInactive()
    {
        foreach (var spriteRenderer in fadeAffected)
        {
            LeanTween.alpha(spriteRenderer.gameObject, inactiveAlpha, .5f).setEaseInQuad();
        }
    }

    public void SetActive()
    {
        foreach (var spriteRenderer in fadeAffected)
        {
            LeanTween.alpha(spriteRenderer.gameObject, 1, .5f).setEaseInQuad();
        }
    }
    public void GrowAnimation()
    {
        if (moveEffect != null)
        {

            var particleScale = big ? bigScale : defaultScale;
            foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
            {
                
               
                LeanTween.value(0, 1, .7f).setEasePunch().setOnUpdate((value) =>
                {
                    var newScale = particleScale + value * 0.2f;
                    ps.transform.localScale = new Vector3(newScale, newScale, newScale);
                });
            }
        }
    }


    public void IncreaseScale(float scale)
    {
        if (moveEffect != null)
        {
        
            Vector3 particleScale= big?new Vector3(bigScale+scale, bigScale+scale, bigScale+scale):new Vector3(defaultScale+scale, defaultScale+scale, defaultScale+scale);
            foreach (var ps in moveEffect.GetComponentsInChildren<ParticleSystem>())
            {
                ps.transform.localScale = particleScale;
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