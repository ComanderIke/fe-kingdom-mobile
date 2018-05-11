using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

    const float delay = 2.0f;
    bool active;
    float time = 0;
	// Use this for initialization
	void Start () {
		
	}
	void OnEnable()
    {
        active = false;
        time = 0;
    }
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadSceneAsync("MainMenu");
                active = false;
            }
        }
        else
        {
            time += Time.deltaTime;
            if (time >= delay)
            {
                active = true;
            }
        }
	}
}
