using UnityEngine;
using System.Collections;

public class MouseWheelInput : MonoBehaviour {

	public delegate void MouseWheelUp ();
	public static MouseWheelUp mouseWheelUp;
	public delegate void MouseWheelDown ();
	public static MouseWheelDown mouseWheelDown;
	// Use this for initialization
	void Start () {
		mouseWheelUp +=Test;
		mouseWheelDown +=Test;
	}
	void Test(){
	}
	float time=1;
	const float BREAK = 0.3f;
	// Update is called once per frame
	void Update () {
		float x= Input.GetAxis("MouseScroll");
	
		time += Time.deltaTime;
		if (time > BREAK&& x !=0) {
			if (x > 0) {
				mouseWheelUp ();
			} else {
				mouseWheelDown ();
			}
			time = 0;
		}
	}
}
