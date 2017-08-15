using UnityEngine;
using System.Collections;

public class ManageCharacters : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void CharacterClicked(Character c){
        GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().SetActiveCharacter(c, false);
		//SendMessageUpwards ("SetActiveCharacter", c);

	}
}
