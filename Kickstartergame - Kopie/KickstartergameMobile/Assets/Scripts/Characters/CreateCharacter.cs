using Assets.Scripts.Characters;
using UnityEngine;

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
    
    public void placeCharacter(int PlayerNumber, LivingObject unit ,int x, int y)
    {
        GameObject o = null;
        string player = "Player" + (PlayerNumber+1);
		o = Instantiate<GameObject>(melee);
        o.name = unit.name;
        o.tag = "Player";
        o.transform.parent = gameObject.transform;
        MovableObject characterScript = o.GetComponentInChildren<MovableObject>();
        characterScript.unit = unit;
        characterScript.GetUnit().gameObject = o;
        o.transform.localPosition = new Vector3(x, y, 0);
        unit.SetPosition(x, y);
    }

    
}
