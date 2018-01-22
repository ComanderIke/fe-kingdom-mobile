using UnityEngine;
using System.Collections;

public class _MainMenu : MonoBehaviour {

	private bool _isMenu = true;

	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 35, Screen.height / 2 - 150, 200, 200), "Demo Menu");
		
		Menu();

		

	}
	
	void Menu()
	{
		if(_isMenu)
		{
			if(GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 25), "Demo 1"))
			{
				Application.LoadLevel("_Demo-1");
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 65, 200, 25), "Demo 2"))
			{
				Application.LoadLevel("_Demo-2");
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 30, 200, 25), "Demo 3"))
			{
				Application.LoadLevel("_Demo-3");
			}


            
        }
	}
	

}
