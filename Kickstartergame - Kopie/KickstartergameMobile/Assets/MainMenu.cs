using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public delegate void StartGameEvent();
	public static StartGameEvent startGameEvent;
	public delegate void ButtonHovered ();
	public static ButtonHovered buttonHovered;
	public delegate void ButtonClicked ();
	public static ButtonHovered buttonClicked;
	bool start=false;
	float starttime=0;
	float alpha=0;
	float time=0;
	bool fadeout=false;
	const float START_TIME = 0.5f;
	// Use this for initialization
	void Start () {
		startGameEvent += Dummy;
		buttonClicked += Dummy;
		buttonHovered += Dummy;
	}
	void Dummy(){

	}
	public void Hovered(){
		buttonHovered ();
	}
	
	// Update is called once per frame
	void Update () {
		
		time += Time.deltaTime;

		//Debug.Log ("alpha" + alpha);
		if (fadeout) {
			alpha = Mathf.Lerp (0, 1, time / 3.0f);
			//Debug.Log (1.0f - (1.0f-alpha));
			GameObject.Find ("Fade").GetComponent<Image> ().color = new Color (0f, 0f, 0f, 1.0f - (1.0f - alpha));
		} else {
			alpha = Mathf.Lerp (0, 1, time / 3.5f);
			//Debug.Log (1.0f-alpha);
			GameObject.Find ("Fade").GetComponent<Image> ().color = new Color (0f, 0f, 0f, 1.0f - alpha);
		}
		if (fadeout && time > 3.5f) {
			Application.LoadLevel ("Level2");
		}
		if (start&& !fadeout) {
			starttime += Time.deltaTime;
			if (starttime >= START_TIME) {
				fadeout = true;
				startGameEvent ();
				time = 0;

			}
		}
	}
	public void StartGame(){
		buttonClicked ();

		GameObject.Find ("gate_wood_animation").GetComponent<Animator> ().SetTrigger ("OpenDoor");
		start = true;

	}
	public void ExitGame(){
		buttonClicked ();
		Application.Quit ();
	}
	public void Options(){
		buttonClicked ();
		Debug.Log ("Options");
	}
}
