using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackPatternUI : MonoBehaviour {

    public TextMeshProUGUI User;
    public TextMeshProUGUI Name;
    bool fadeout = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !fadeout)
        {
            fadeout = true;
            StartCoroutine(FadeOut());
        }
	}
    void OnEnable()
    {
        //StartCoroutine(FadeOut());
    }
    public void Show(string userText, string NameText)
    {
        gameObject.SetActive(true);
        User.text = userText+ " counters with";
        Name.text = NameText+"!";
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.3f);
        fadeout = false;
        EventContainer.continuePressed();
        gameObject.SetActive(false);
    }
}
