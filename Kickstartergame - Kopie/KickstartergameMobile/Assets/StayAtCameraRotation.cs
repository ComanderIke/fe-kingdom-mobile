using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAtCameraRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(42, 32, 0);
	}
}
