using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Message : MonoBehaviour {

	private bool shown = false;
	private float time;
	private string message;
	public void Show(string message)
	{
		shown = true;
		time = 0;
		message = message;
		GetComponent<AudioSource>().Play();
		gameObject.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
		gameObject.GetComponentInChildren<Text>().text = message;
	}
	void Update()
	{
		if (shown)
		{
			time += Time.deltaTime;
		}
		if (time > 1.7f)
		{
			time = 0;
			shown = false;
			gameObject.transform.position = new Vector3(-500, Screen.height / 2, 0);
		}
	}
}
