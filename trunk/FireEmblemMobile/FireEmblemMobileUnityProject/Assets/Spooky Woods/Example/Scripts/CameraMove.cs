using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public float speed = 5.0f;

	void Update () 
	{

		transform.Translate(speed * Time.deltaTime, 0, 0);
	
	}
}
