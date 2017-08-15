using UnityEngine;
using System.Collections;

public class ToogleSound : MonoBehaviour {

	// Use this for initialization
	public AudioListener audioListener;
	private bool enabled=true;
	public float volume;
	void Start () {
	
	}
	public void OnButtonClick(){
		if (enabled) {
			enabled = false;
			AudioListener.volume = 0;
		}
		else{
			enabled = true;
			AudioListener.volume = volume;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
