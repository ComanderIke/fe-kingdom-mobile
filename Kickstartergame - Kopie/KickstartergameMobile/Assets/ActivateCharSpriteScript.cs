using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActivateCharSpriteScript : MonoBehaviour {

	public static bool hide=false;
	GameObject spritesSwitch;
    Image backGround;
	// Use this for initialization
	void Start () {
		spritesSwitch = GameObject.Find ("CharSpritesSwitch");
        backGround = GameObject.Find("CharSpritesBackground").GetComponent<Image>();

    }
	
	public void OnClick() {
		if (hide) {
            hide = false;
            spritesSwitch.SetActive(false);
            backGround.enabled = false;
        } else {
			hide = true;
            backGround.enabled = true;
            spritesSwitch.SetActive(true);
        }
	}
}
