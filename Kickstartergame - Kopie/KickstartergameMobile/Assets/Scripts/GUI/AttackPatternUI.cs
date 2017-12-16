using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPatternUI : MonoBehaviour {

    public Text User;
    public Text Name;
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
        this.User.text = userText+ " uses";
        this.Name.text = NameText+"!";
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.3f);
        fadeout = false;
        EventContainer.continuePressed();
        gameObject.SetActive(false);
    }
}
