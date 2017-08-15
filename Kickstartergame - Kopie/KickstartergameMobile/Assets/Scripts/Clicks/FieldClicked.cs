using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AssemblyCSharp;

public class FieldClicked : MonoBehaviour {

	public Vector3 position;
	public bool hovered = false;
	const float HOVER_TIME=0.5f;
	bool mouse_hovered=false;
	float hovertime=0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (hovered) {
            if (MyInputManager.IsAButtonDown())
            {
                SendMessageUpwards("FieldClicked", new Vector2(position.x, position.z));
            }
		}
		if (mouse_hovered) {
			hovertime += Time.deltaTime;
			if (hovertime >= HOVER_TIME) {
				MainScript.fieldHoveredEvent ((int)position.x, (int)position.z);
			}
		}*/
	}
	void OnMouseOver(){
		//mouse_hovered = true;
	}
   /* void OnMouseDown() {
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			MainScript.clickedOnField((int)position.x, (int)position.z);
			SendMessageUpwards ("FieldClicked", new Vector2 (position.x, position.z));
		}
    }*/
    Texture temp;
    void OnMouseEnter()
    {
		//hovertime = 0;
        //GameObject.Find("MouseCursor").transform.position = new Vector3(position.x+0.5f, position.y, position.z+0.5f) ;
    }
    void OnMouseExit()
    {
		//MainScript.itemDescription.SetActive (false);
		//MainScript.weaponDescriptionLong.SetActive (false);
		//MainScript.weaponDescriptionShort.SetActive (false);
		/*GameObject item =GameObject.Find ("ItemName");
		if(item!=null)
			item.GetComponent<Text> ().text = "";
		GameObject[] gos=GameObject.FindGameObjectsWithTag ("3DItem");
		foreach (GameObject go in gos) {
			Destroy (go);
		}
		ItemDrop.active = false;
		mouse_hovered = false;*/
    }
}
