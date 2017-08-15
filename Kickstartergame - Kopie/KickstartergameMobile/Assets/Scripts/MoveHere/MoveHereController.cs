using UnityEngine;
using System.Collections;

public class MoveHereController : MonoBehaviour {

	static private GameObject go_LeilaFirstBattle;

	void Start () {
		go_LeilaFirstBattle = GameObject.Find ("MoveHere_LeilaFirstBattle");
		go_LeilaFirstBattle.SetActive (false);
	}

	static public void LeilaFirstBattleStart() {
		go_LeilaFirstBattle.SetActive (true);
	}

	static public void LeilaFirstBattleEnd() {
		go_LeilaFirstBattle.SetActive (false);
	}
}
