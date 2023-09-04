using System.Collections.Generic;
using UnityEngine;

public class ActiveUnitGroundFX:MonoBehaviour
{
    [SerializeField]private List<SpriteRenderer> fadeSprites;
    public void FadeIn()
    {
        Debug.Log("FadeIn ACTIVE UNIT GOUND FX");
        foreach (var sprite in fadeSprites)
        {
           // LeanTween.cancel(sprite.gameObject);
            LeanTween.alpha(sprite.gameObject, 1f, 1.5f).setEaseInOutQuad();
        }
    }
    public void FadeOut()
    {
        Debug.Log("FADE OUT ACTIVE UNIT GOUND FX");
        //LeanTween.alpha(fadeSprites[0].gameObject, 0f, .2f).setEaseInOutQuad();
        foreach (var sprite in fadeSprites)
        {
           // LeanTween.cancel(sprite.gameObject);
           Debug.Log("Start Tween for: "+sprite.gameObject);
          // sprite.color = new Color(1, 1, 1, 0);
            LeanTween.alpha(sprite.gameObject, 0f, .3f).setEaseInOutQuad();
        }
    }
}