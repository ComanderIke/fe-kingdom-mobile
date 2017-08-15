using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApertureHeight : MonoBehaviour {

    public float height;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.parent.transform.position.x + (transform.parent.transform.position.y*0.6f), 0, transform.parent.transform.position.z + (transform.parent.transform.position.y * 1));
	}
}
