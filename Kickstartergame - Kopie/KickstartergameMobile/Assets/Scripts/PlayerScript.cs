using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Characters;

[System.Serializable]
public class Player{
	public int number;
	public Color color;
	public string name;
    public bool isPlayerControlled;
    [System.NonSerialized]
	private List<LivingObject> units;

    public Color GetColor()
    {
        
        return color;
    }
	public Player(int number, Color color, string name, bool isPlayerControlled){
		this.number = number;
        this.name = name;
        this.isPlayerControlled = isPlayerControlled;
		this.color = color;
        
	}
	public List<LivingObject> getCharacters(){
		return units;
	}
    public void addCharacter(Character c)
    {
        if(units==null)
            units = new List<LivingObject>();
        c.team = number;
        c.player = this;
        units.Add((LivingObject)c);

    }
    public void setCharacters(List<LivingObject> c){
		units = c;
	}




}

public class PlayerScript : MonoBehaviour {

	public Player player;


}
