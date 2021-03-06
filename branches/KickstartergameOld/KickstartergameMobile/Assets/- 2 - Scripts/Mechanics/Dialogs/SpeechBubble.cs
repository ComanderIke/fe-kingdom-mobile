﻿using BeautifulTransitions.Scripts.Transitions.Components.GameObject;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour {

    TextMeshProUGUI text;
    TextAnimation textAnimation;
	// Use this for initialization
	void Awake () {
        text = GetComponentInChildren<TextMeshProUGUI>();
        textAnimation = GetComponentInChildren<TextAnimation>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Show(string text)
    {
        this.gameObject.SetActive(true);
        GetComponent<TransitionScale>().TransitionIn();
        
        this.text.text = text;
        textAnimation.StartAnimation();
        StartCoroutine(Hide());

    }
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(2);
        GetComponent<TransitionScale>().TransitionOut();

       // this.gameObject.SetActive(false);
    }
}
