
using Assets.Scripts.Characters;
using Assets.Scripts.Items.Weapons;
using UnityEngine;

public class UnitInstantiater : MonoBehaviour {

	public GameObject melee;
    public GameObject lancer;
    public GameObject archer;
    public GameObject mammoth;
    public GameObject axefighter;
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
            Human h = (Human) unit;
            if(h.EquipedWeapon.WeaponType == WeaponType.Sword)
            {
                o = Instantiate<GameObject>(melee);

            }else if (h.EquipedWeapon.WeaponType == WeaponType.Bow)
            {
                o = Instantiate<GameObject>(archer);
            }
            else if (h.EquipedWeapon.WeaponType == WeaponType.Spear)
            {
                o = Instantiate<GameObject>(lancer);
            }
            else if (h.EquipedWeapon.WeaponType == WeaponType.Axe)
            {
                o = Instantiate<GameObject>(axefighter);
            }

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
