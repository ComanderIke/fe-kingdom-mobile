using UnityEngine;
using System.Collections;

public class MainCameraScript : MonoBehaviour {

    Vector3 oldPosition;
	// Use this for initialization
	void Start () {
	
	}
    public bool isMoving;
	// Update is called once per frame
	void Update () {
	    if(transform.position == oldPosition)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
        oldPosition = transform.position;
	}
}
