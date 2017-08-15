using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCamera : MonoBehaviour {

    public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {

        transform.SetParent(target.GetComponentInChildren<BattleCamPosition>().transform, false);
        transform.localPosition = new Vector3();
        transform.localRotation = Quaternion.identity;
    }
}
