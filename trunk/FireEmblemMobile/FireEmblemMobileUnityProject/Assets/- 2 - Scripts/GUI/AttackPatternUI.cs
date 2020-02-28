using System.Collections;
using TMPro;
using UnityEngine;

public class AttackPatternUI : MonoBehaviour
{

    public TextMeshProUGUI User;
    public TextMeshProUGUI Name;
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && !fadeout)
        //{
        //    fadeout = true;
        //    StartCoroutine(FadeOut());
        //}
    }
    void OnEnable()
    {
        //StartCoroutine(FadeOut());
    }
    public void Show(string userText, string NameText)
    {
        //gameObject.SetActive(true);
        //User.text = userText+ " counters with";
        //Name.text = NameText+"!";
        MainScript.instance.StartCoroutine(FadeOut(0.1f));

    }
    IEnumerator FadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        UISystem.onContinuePressed();
        gameObject.SetActive(false);
    }
}