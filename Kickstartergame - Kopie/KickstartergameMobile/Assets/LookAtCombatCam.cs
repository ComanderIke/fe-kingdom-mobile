using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCombatCam : MonoBehaviour {
    Camera combatCam;
	// Use this for initialization
	void Start () {
		
	}
    private void OnEnable()
    {
        //Debug.Log(GameObject.Find("CombatCam").name);
        combatCam = GameObject.Find("CombatCamera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update () {
        transform.LookAt(combatCam.transform);
	}
}
