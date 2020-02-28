using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public Army player;
	// Use this for initialization
	void Start () {
        player = new Army(0, "Player 1", true);
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddUnit(Unit unit)
    {
        player.AddUnit(unit);
    }
}
