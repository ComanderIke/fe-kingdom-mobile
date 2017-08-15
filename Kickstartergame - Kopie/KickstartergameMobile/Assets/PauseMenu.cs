using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {

        Time.timeScale = 0.0f;
    }
    private void OnDisable()
    {

        Time.timeScale = 1;
    }
    public void Continue()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
    }
    public void Options()
    {
        
    }
    public void Retreat()
    {

    }
    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("BackgroundMenu");
    }
}
