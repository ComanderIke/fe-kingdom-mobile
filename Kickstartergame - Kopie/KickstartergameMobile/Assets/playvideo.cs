using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playvideo : MonoBehaviour {

	public MovieTexture movie;
	private AudioSource audio;
	bool startmovie=false;
	float fadetime=0;
	float fadealpha=0;
	// Use this for initialization
	void Start () {
		
	}
	void StartMovie(){
		startmovie = true;
		GetComponent<RawImage> ().enabled = true;
		GetComponent<RawImage> ().texture = movie as MovieTexture;
		audio = GetComponent<AudioSource>();
		audio.clip = movie.audioClip;
		movie.Play();
		audio.Play ();
	}
	// Update is called once per frame
	void Update () {
		fadetime += Time.deltaTime;
		if (fadetime <= 0.8f) {
			fadealpha = Mathf.Lerp (0.0f, 1.0f, fadetime / 0.8f);
			GameObject.Find ("Fade").GetComponent<Image> ().color = new Color (0, 0, 0, fadealpha);
		}
		if (fadetime >= 2&&!startmovie) {
			StartMovie ();
		}
		if(!movie.isPlaying&&startmovie){
			Application.LoadLevel ("BackgroundMenu");
		}
	}
}
