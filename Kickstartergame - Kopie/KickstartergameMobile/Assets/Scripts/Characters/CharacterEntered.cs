using UnityEngine;
using System.Collections;

public class CharacterEntered : MonoBehaviour {

	public int number;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
    void OnTriggerEnter(Collider c)
    {
        CharacterScript cs = c.gameObject.GetComponent<CharacterScript>();
        if (cs != null)
        {
               // Destroy(this.gameObject);
			GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>().CharacterEnteredRoom(number, cs.character);
        }
    }
}
