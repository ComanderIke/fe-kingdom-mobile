using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenParticleSystem : MonoBehaviour {

    [SerializeField]
    private GameObject ps_test;
    [SerializeField]
    private Camera ps_Camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = ps_Camera.ScreenToWorldPoint(Input.mousePosition);
            GameObject ps= GameObject.Instantiate(ps_test, position, Quaternion.identity);
            ps.layer = LayerMask.NameToLayer("FrontUI");
            ps.AddComponent<ParticleSystemAutoDestroy>();
            foreach(Transform trans in ps.transform.GetComponentsInChildren<Transform>())
            {
                trans.gameObject.layer = LayerMask.NameToLayer("FrontUI");
            }
        }
	}
}
