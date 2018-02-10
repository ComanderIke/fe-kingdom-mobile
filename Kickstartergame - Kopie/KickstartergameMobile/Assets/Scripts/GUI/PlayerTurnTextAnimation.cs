using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnTextAnimation : MonoBehaviour {

    public TextMeshProUGUI text;
    const float FADE_IN_DURATION = 0.3f;
    const float FADE_OUT_DURATION = 0.4f;
    const float OPAQUE_DURATION = 0.8f;
    public static float duration = FADE_IN_DURATION + OPAQUE_DURATION + FADE_OUT_DURATION;
    // Use this for initialization
    void Start () {
        StartCoroutine(Fade());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Fade()
    {
        float delay = FADE_IN_DURATION;
        float alpha = 0;
        while (delay > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha += 0.01f/ FADE_IN_DURATION;
            delay -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        yield return new WaitForSeconds(OPAQUE_DURATION);
        delay = FADE_OUT_DURATION;
        alpha = 1;
        while (delay > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha -= 0.01f/ FADE_OUT_DURATION;
            delay -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        gameObject.SetActive(false);
    }

}
