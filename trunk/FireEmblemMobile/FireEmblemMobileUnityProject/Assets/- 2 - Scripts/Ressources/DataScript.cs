using UnityEngine;
using System.Collections;
using Assets.Scripts.Items;
using Assets.Scripts.Characters;
using System.Collections.Generic;
using Assets.Scripts.Items.Weapons;
using Assets.Scripts.Ressources;

public class DataScript : MonoBehaviour {

    //public Weapons weapons;
    public MapData mapData;
    public DialogData dialogTexts;
    public UnitData unitData;
    
    public static DataScript instance;
    [SerializeField]
    List<Weapon> weapons;
    [SerializeField]
    List<Human> humans;
    [SerializeField]
    List<Monster> monster;

    public List<Weapon> Weapons
    {
        get
        {
            List<Weapon> ret = new List<Weapon>();
            for (int i = 0; i < weapons.Count; i++)
            {
                Weapon at = GameObject.Instantiate(weapons[i]);
                at.name = weapons[i].name;
                ret.Add(at);
            }
            return ret;
        }
    }
    public Weapon GetWeapon(string name)
    {
        return GameObject.Instantiate(weapons.Find(a => a.name == name));
    }
    public Human GetHuman(string name)
    {
        return GameObject.Instantiate(humans.Find(a => a.Name == name));
    }
    public Monster GetMonster(string name)
    {
        return GameObject.Instantiate(monster.Find(a => a.Name == name));
    }

    private void OnEnable()
    {
        instance = this;
    }
    private void OnDestroy()
    {
        instance = null;
    }


}
