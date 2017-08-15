using UnityEngine;
using System.Collections;

public class ItemRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(new Vector3(50*Time.deltaTime,0,0));
	}
}
