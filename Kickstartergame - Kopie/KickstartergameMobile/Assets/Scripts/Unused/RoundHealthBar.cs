using UnityEngine;
using System.Collections;

public class RoundHealthBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    float speed = 1f;
	// Update is called once per frame
	void Update () {
        transform.RotateAround(new Vector3(0, 1, 0), speed * Time.deltaTime);
	}
}
