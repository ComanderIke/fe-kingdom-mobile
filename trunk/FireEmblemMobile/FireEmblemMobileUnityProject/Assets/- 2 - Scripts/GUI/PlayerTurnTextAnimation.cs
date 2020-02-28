using BeautifulTransitions.Scripts.Transitions.Components;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnTextAnimation : MonoBehaviour {

    public TextMeshProUGUI text;
    public Image backGround;
    const float FADE_IN_DURATION = 0.2f;
    const float FADE_OUT_DURATION = 0.4f;
    const float OPAQUE_DURATION = 0.4f;
    private float backGroundMaxAlpha = 0;
    public static float duration = FADE_IN_DURATION + OPAQUE_DURATION + FADE_OUT_DURATION;
    // Use this for initialization
    void Start () {
        backGroundMaxAlpha = backGround.color.a;
        StartCoroutine(Fade());
        
	}
	
    IEnumerator Fade()
    {
        float delay = FADE_IN_DURATION;
        float alpha = 0;
        while (delay > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, Mathf.Clamp(alpha,0,backGroundMaxAlpha));
            alpha += 0.01f/ FADE_IN_DURATION;
            delay -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, backGroundMaxAlpha);
        yield return new WaitForSeconds(OPAQUE_DURATION);
        delay = FADE_OUT_DURATION;
        alpha = 1;
        gameObject.transform.parent.GetComponent<TransitionBase>().TransitionOut();
        while (delay > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, Mathf.Clamp(alpha, 0, backGroundMaxAlpha));
            alpha -= 0.01f/ FADE_OUT_DURATION;
            delay -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        backGround.color = new Color(backGround.color.r, backGround.color.g, backGround.color.b, 0);
        Destroy(transform.parent.gameObject);
    }

}
