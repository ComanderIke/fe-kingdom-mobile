using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeLevel : MonoBehaviour {

    public GameObject ressourcePrefab;
    public GameObject gameDataPrefab;
    public GameObject audioPrefab;
	// Use this for initialization
	void Awake () {
        if (GameObject.Find("RessourceScript") == null)
        {
            GameObject go = GameObject.Instantiate(ressourcePrefab);
            go.name = "RessourceScript";
        }
        if (GameObject.Find("GameData") == null)
        {
            GameObject go = GameObject.Instantiate(gameDataPrefab);
            go.name = "GameData";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
