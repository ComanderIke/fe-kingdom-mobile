using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField]
    GameObject bottomUI;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void HideBottomUI()
    {
        bottomUI.SetActive(false);
    }
    public void ShowBottomUI()
    {
        bottomUI.SetActive(true);
    }
}
