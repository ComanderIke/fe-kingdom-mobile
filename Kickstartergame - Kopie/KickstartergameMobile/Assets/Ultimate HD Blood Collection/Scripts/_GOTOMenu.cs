using UnityEngine;
using System.Collections;

public class _GOTOMenu : MonoBehaviour {

	private bool _isMenu = true;
	// Use this for initialization
	void OnGUI()
	{
		//GUI.Label(new Rect(Screen.width / 2 - 35, Screen.height / 2 - 150, 200, 200), "Demo Menu");
		
		Menu();
		
		
		
	}
	
	void Menu()
	{
		if(_isMenu)
		{

			
			if (GUI.Button(new Rect(10, Screen.height / 2 - 30, 200, 25), "Menu"))
			{
				Application.LoadLevel("_Menu");
			}
			
			
			
		}
	}
}
