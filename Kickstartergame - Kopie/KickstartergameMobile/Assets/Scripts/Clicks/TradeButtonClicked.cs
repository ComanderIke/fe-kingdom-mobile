using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TradeButtonClicked : MonoBehaviour {

	public bool hovered=false;
	Button b;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //if (MyInputManager.GAMEPAD_INPUT)
        //{
            if (hovered && MyInputManager.IsAButtonDown())
            {

                SendMessageUpwards("TradeButtonClicked", b);
            }
        //}
        //else
        //{
        //    if (hovered && ControllerServer.getButton1(MainScript.ActivePlayerNumber))
        //    {
        //        SendMessageUpwards("ItemButtonClicked", b);
        //    }
        //}
	}
	public void OnHover(Button o)
	{
		b = o;
		hovered = true;
		if (o.IsInteractable()) {
			Text t = o.GetComponentInChildren<Text>();
			Debug.Log ("Test:" +t.text);
			t.color = Color.white;
			//AudioSource a = o.GetComponentInChildren<AudioSource>();
			//a.Play();
		}
	}
	public void OnNoHover(Button o)
	{
		hovered = false;
		Text t = o.GetComponentInChildren<Text>();
		t.color = Color.black;
					Debug.Log ("Test:" +t.text);
	}

}
