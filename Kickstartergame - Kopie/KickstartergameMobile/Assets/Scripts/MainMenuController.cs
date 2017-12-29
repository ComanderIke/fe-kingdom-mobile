using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Exit()
    {
        StartCoroutine(DelayQuit(1.0f));
    }
    public void StartGame()
    {
        StartCoroutine(DelayStart(1.0f)); 
    }
    public void Options()
    {
        //TODO
    }
    public void Base()
    {
        StartCoroutine(DelayBase(1.0f));
    }
    IEnumerator DelayBase(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Base");
    }
    IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("CharacterCreater");
    }
    IEnumerator DelayQuit(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
