using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnTextAnimation : MonoBehaviour {

    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        StartCoroutine(Fade());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Fade()
    {
        float startdelay = 0.2f;
        float delay = startdelay;
        float alpha = 0;
        while (delay > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha += 0.01f/startdelay;
            delay -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        yield return new WaitForSeconds(1.0f);
        startdelay = 1.0f;
        delay = startdelay;
        alpha = 1;
        while (delay > 0)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            alpha -= 0.01f/startdelay;
            yield return new WaitForSeconds(0.01f);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

    }

}
