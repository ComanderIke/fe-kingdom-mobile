using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToogleHPBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Toogle(){
		HPBarOnMap.active = !HPBarOnMap.active;
	}
}
