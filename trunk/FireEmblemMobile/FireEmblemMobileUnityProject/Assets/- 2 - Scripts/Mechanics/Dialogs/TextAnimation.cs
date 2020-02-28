using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour {

    private int letterindex = 0;
    string text;
    public TextMeshProUGUI textMesh;
    const float speed = 0.03f;
    IEnumerator enumerator;
    // Use this for initialization
    void Awake () {
        textMesh = GetComponent<TextMeshProUGUI>();
	}
	public void StartAnimation()
    {
        letterindex = 0;
        text = textMesh.text;
        textMesh.text = "";
        
        enumerator = DisplayTimer();
        StartCoroutine(enumerator);

    }
    IEnumerator DisplayTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(speed);
            if (letterindex > text.Length)
            {
                continue;
            }
            textMesh.text = text.Substring(0, letterindex);
            letterindex++;
            GetComponent<AudioSource>().Play();
        }
    }
    public void StopAnimation()
    {
        StopCoroutine(enumerator);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
