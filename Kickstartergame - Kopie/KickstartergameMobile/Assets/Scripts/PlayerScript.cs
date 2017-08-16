using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters.Classes;
using Assets.Scripts;
[System.Serializable]
public class Player{
	public int number;
	public Color color;
	public string name;
    public bool isPlayerControlled;
    [System.NonSerialized]
	private List<Character> characters;

    public Color GetColor()
    {
        
        return color;
    }
	public Player(int number, Color color, string name, bool isPlayerControlled){
		this.number = number;
        this.name = name;
        this.isPlayerControlled = isPlayerControlled;
		this.color = color;
        characters = new List<Character>();
	}

	public void Init(){
		characters= new List<Character> ();
	}
	public List<Character> getCharacters(){
		return characters;
	}
    public void addCharacter(Character c)
    {
        c.team = number;
        c.player = this;
        characters.Add(c);
    }
    public void setCharacters(List<Character> c){
		characters = c;
	}




}

public class PlayerScript : MonoBehaviour {

	public Player player;

    void Start() {
        player.Init();
        List<Character> characters = new List<Character>();
    }

}
