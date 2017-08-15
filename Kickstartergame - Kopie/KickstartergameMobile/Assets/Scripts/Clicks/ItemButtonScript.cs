using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButtonScript : MonoBehaviour {

	// Use this for initialization
	public bool hovered=false;
	Button b;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (MyInputManager.GAMEPAD_INPUT)
        //{
            //if (hovered && MyInputManager.IsAButtonDown())
            //{
            //    SendMessageUpwards("ItemButtonClicked", b);
            //}
        //}
        //else
        //{
        //    if (hovered && ControllerServer.getButton1(MainScript.ActivePlayerNumber))
        //    {
        //        SendMessageUpwards("ItemButtonClicked", b);
        //    }
        //}
	}
	//public void OnClick(Button o){
	//	SendMessageUpwards ("ItemButtonClicked",o);
	//}
    public void OnHover(Button o)
    {
		b = o;
		hovered = true;
        if (o.IsInteractable()) {
            Text t = o.GetComponentInChildren<Text>();
            t.color = Color.white;
            transform.localScale = new Vector3(1.33f, 1.33f, 1.33f);
            //AudioSource a = o.GetComponentInChildren<AudioSource>();
            //a.Play();
        }
    }
    public void OnNoHover(Button o)
    {
		hovered = false;
        Text t = o.GetComponentInChildren<Text>();
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        t.color = Color.black;
    }
}
