using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {

	public float strength;
	float originalY;
	float originalRot;
	Vector3 floatY;
	// Use this for initialization
	void Start () {
		this.originalY = this.transform.position.y;
		transform.Rotate (new Vector3 (0, 0, -5));
	}
	
	// Update is called once per frame
	void Update () {
		floatY = transform.position;
		floatY.y = originalY + (Mathf.Sin (Time.time*3) * strength);
		this.transform.position = floatY;
		transform.Rotate (new Vector3 (0, 0, Mathf.Sin(Time.time*4)*15*Time.deltaTime));
	}
}
