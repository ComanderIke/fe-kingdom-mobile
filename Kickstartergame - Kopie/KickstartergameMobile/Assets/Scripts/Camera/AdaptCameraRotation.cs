using UnityEngine;
using System.Collections;

public class AdaptCameraRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, GameObject.Find("Main Camera").transform.eulerAngles.y, 0));
	}
}
