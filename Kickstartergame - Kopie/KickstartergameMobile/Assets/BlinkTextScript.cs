using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkTextScript : MonoBehaviour {

    private Text text;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
	}
    private void OnEnable()
    {
        StartCoroutine(Blink());
    }
    private void OnDisable()
    {
        StopCoroutine("Blink");
    }
    IEnumerator Blink()
    {
        while (true)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            yield return new WaitForSeconds(0.25f);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            yield return new WaitForSeconds(0.75f);
        }
    }
}
