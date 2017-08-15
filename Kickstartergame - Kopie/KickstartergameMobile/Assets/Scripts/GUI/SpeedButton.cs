using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClick()
    {
        if (Time.timeScale == 1.0f)
        {
            GetComponentInChildren<Text>().text = "Speedx0.5";
            Time.timeScale = 2.0f;
        }
        else if (Time.timeScale==2.0f)
        {
            GetComponentInChildren<Text>().text = "Speedx1";
            Time.timeScale = 0.5f;
        }
        else
        {
            GetComponentInChildren<Text>().text = "Speedx2";
            Time.timeScale = 1f;
        }
    }
}
