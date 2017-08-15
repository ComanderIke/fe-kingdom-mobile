using UnityEngine;
using System.Collections;

public class CursorMovScript : MonoBehaviour {

	public MovieTexture movtexture;
	// Use this for initialization
	void Start () {
		this.GetComponent<MeshRenderer> ().material.mainTexture = movtexture;
		movtexture.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
