using UnityEngine;
using System.Collections;

public class ArrowCollision : MonoBehaviour {

	public static Character shooter;
	public static Character target;
    public GameObject hitEffect;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision col)
    {
		Debug.Log("Collision" + col.collider.gameObject.name);
        Vector3 pos = gameObject.transform.position;
		if(target!=null)
		//Debug.Log (target.name);
		if (target!=null&&col.gameObject.GetComponent<CharacterScript> () != null && col.gameObject.GetComponent<CharacterScript> ().character == target) {
			Debug.Log ("YOYOOOO");
			shooter.gameObject.GetComponentInChildren<AnimationEventController> ().OnAttackHit ();
			if (hitEffect != null)
				Instantiate (hitEffect, pos, Quaternion.identity);
			Destroy (gameObject);
		}
    }
}
