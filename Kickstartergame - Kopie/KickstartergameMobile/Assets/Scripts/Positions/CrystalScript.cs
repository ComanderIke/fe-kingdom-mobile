using UnityEngine;
using System.Collections;
[System.Serializable]
public enum CrystalType{
	Health,
	Mana,
	Exp
}
public class CrystalScript : MonoBehaviour {

	public CrystalType type;
	public int crystals=1;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public int GetX()
	{
		return (int)transform.position.x;
	}
	public int GetZ()
	{
		return (int)transform.position.z-1;
	}
	public void Take(Character c){
		if (crystals <= 0)
			return;
		Item item=null;
		switch (type) {
			case CrystalType.Health:
				item = GameObject.Find ("RessourceScript").GetComponent<ItemSpriteScript> ().CreateHealthCrystal ();
				break;
			case CrystalType.Mana:
				item = GameObject.Find ("RessourceScript").GetComponent<ItemSpriteScript> ().CreateManaCrystal ();
				break;
			case CrystalType.Exp:
				item = GameObject.Find ("RessourceScript").GetComponent<ItemSpriteScript> ().CreateExpCrystal ();
				break;

		}
		c.addItem (item);
		crystals--;
	}
}
