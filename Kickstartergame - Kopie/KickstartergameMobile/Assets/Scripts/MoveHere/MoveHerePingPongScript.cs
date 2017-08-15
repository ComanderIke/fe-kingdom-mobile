using UnityEngine;
using System.Collections;

public class MoveHerePingPongScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {

		float currentPong = Mathf.PingPong (Time.time, 1);

		if (currentPong > 0.7f) {
			transform.localScale = new Vector3 (currentPong/10, transform.localScale.y, currentPong/10);
		}
	}
}
