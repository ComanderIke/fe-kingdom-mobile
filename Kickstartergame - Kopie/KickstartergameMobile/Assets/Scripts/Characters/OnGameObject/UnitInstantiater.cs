using Assets.Scripts.Characters;
using UnityEngine;

public class UnitInstantiater : MonoBehaviour {

	public GameObject melee;
    public GameObject mage;
    public GameObject archer;
    public GameObject mammoth;
    public GameObject hellebardier;
    public GameObject sabertooth;
    
    public void PlaceCharacter(int playerNumber, LivingObject unit ,int x, int y)
    {
        GameObject o = null;
        string player = "Player" + (playerNumber+1);
        if (unit is Monster)
        {
            if (((Monster)unit).Type == MonsterType.Mammoth)
                o = Instantiate<GameObject>(mammoth);
            else if (((Monster)unit).Type == MonsterType.Sabertooth)
                o = Instantiate<GameObject>(sabertooth);
        }
        else
        {
            o = Instantiate<GameObject>(melee);
            o.GetComponentInChildren<SpriteRenderer>().sprite = unit.Sprite;
        }
        o.name = unit.Name;
        o.transform.parent = gameObject.transform;
        UnitController unitController = o.GetComponentInChildren<UnitController>();
        unitController.Unit = unit;
        unit.GameTransform.GameObject = o;
        unit.SetPosition(x, y);
    }
}
