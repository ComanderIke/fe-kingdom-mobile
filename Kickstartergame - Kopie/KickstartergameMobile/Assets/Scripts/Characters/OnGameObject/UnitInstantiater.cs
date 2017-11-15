using Assets.Scripts.Characters;
using UnityEngine;

public class UnitInstantiater : MonoBehaviour {

	public GameObject melee;
    public GameObject mage;
    public GameObject archer;
    public GameObject mammoth;
    public GameObject hellebardier;
    
    public void PlaceCharacter(int playerNumber, LivingObject unit ,int x, int y)
    {
        GameObject o = null;
        string player = "Player" + (playerNumber+1);
        if (unit is Monster)
        {
            o = Instantiate<GameObject>(mammoth);
        }
        else
        {
            o = Instantiate<GameObject>(melee);
        }
        o.name = unit.Name;
        o.transform.parent = gameObject.transform;
        MovableObject characterScript = o.GetComponentInChildren<MovableObject>();
        characterScript.Unit = unit;
        unit.GameTransform.GameObject = o;
        unit.SetPosition(x, y);
    }
}
