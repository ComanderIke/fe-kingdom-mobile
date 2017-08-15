using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBarOnMap : MonoBehaviour {
	public Character character;
	private float scaleFactor = 1;
	private float currentValue;
    public bool isArmor = false;
    public bool isMana = false;
    public bool isMagicShield = false;
	public float healthSpeed;
	public static bool active = true;
	private Transform [] transforms;
	// Use this for initialization
	void Start () {	
		transforms = this.gameObject.GetComponentsInChildren<Transform> ();
        character=this.transform.parent.gameObject.GetComponent<CharacterScript>().character;
        foreach(Transform go in GetComponentsInChildren<Transform>())
        {
            go.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
		/*if (active) {
			if (character.HP == character.stats.maxHP) {
				active = false;
			}
			if (character != null) {
                if (character.HP != 0)
                {
                    currentValue = MapValues(character.HP, 0f, character.stats.maxHP, 0f, 1f);

                    scaleFactor = Mathf.Lerp(scaleFactor, currentValue, Time.deltaTime * healthSpeed);
                    this.gameObject.transform.localScale = new Vector3(scaleFactor, 1, 1);
                }
                else
                {
                    scaleFactor = 0;

                    this.gameObject.transform.localScale = new Vector3(scaleFactor, 1, 1);
                }
			}
		} */
			
	}
	public void SetActive(Character c){
		//character = c;
	}
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax ){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
