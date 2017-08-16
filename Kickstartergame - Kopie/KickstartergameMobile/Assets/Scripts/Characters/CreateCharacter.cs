using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters.Classes;

public class CreateCharacter : MonoBehaviour {

	public GameObject melee;
    public GameObject mage;
    public GameObject archer;
    public GameObject hellebardier;
    public GameObject meleeWeaponPosition;
    public GameObject rangedWeaponPosition;
    public GameObject mageWeaponPosition;
    // Use this for initialization
    void Start () {
		

	}
    
    public void placeCharacter(int PlayerNumber, Character c ,int x, int y)
    {
        GameObject o = null;
        string player = "Player" + (PlayerNumber+1);
        if (c.characterClassType == CharacterClassType.SwordFighter)
        {
			o = Instantiate<GameObject>(melee);

        }
        if (c.characterClassType == CharacterClassType.Hellebardier)
        {
            o = Instantiate<GameObject>(hellebardier);

        }
        if (c.characterClassType == CharacterClassType.Archer)
        {
			o = Instantiate<GameObject>(archer);
        }
        if (c.characterClassType == CharacterClassType.Mage) {
			o = Instantiate<GameObject>(mage);
        }
        
        o.name = c.name;
        o.tag = "Player";
        o.transform.parent = gameObject.transform;
        CharacterScript characterScript = o.GetComponentInChildren<CharacterScript>();
        characterScript.character=c;
        characterScript.getCharacter().gameObject = o;
        o.transform.localPosition = new Vector3(x, y, 0);
        c.SetPosition(x, y);
        c.InstantiateWeapon();
    }

    
}
