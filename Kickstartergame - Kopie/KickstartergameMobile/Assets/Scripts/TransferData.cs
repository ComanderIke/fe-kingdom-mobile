using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum Difficulty{
	Easy,
	Normal,
	Hard
}
public class TransferData : MonoBehaviour {

    // Use this for initialization
	public static Difficulty difficulty = Difficulty.Easy;
    public Player player;
    void Start () {
        player = new Player(0,Color.blue,"testplayer", true);
        player.setCharacters(new List<Character>());
    }
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
