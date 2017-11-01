using UnityEngine;
using Assets.Scripts.Characters;

public enum AIBehaviour
{
    passiv,
    normal,
    aggressiv,
    guard,
}
public enum ItemEnum{
	Recurvebow,
	Arkum,
	Valkyrie,
	Dragonstinger,
	SpearPrincess,
	SteelLance,
	Knightsword,
	Avenger,
	Bastardsword,
	Deathbringer,
	Stiletto,
	HuntingKnife,
	Ignis,
	Ventus,
	ThanatosBreath,
	Waraxe,
	Slasher,
	Healthpotion,
	Healthcrystal,
	Manacrystal,
	Experiencecrystal,
	DoorKey
}
public class EnemyPosition : MonoBehaviour {

    public MonsterType charType;
	public int level;
	// Use this for initialization
	void Start () {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
	public int GetLevel(){
		return level;
	}
    public int GetX()
    {
        return (int)transform.localPosition.x;
    }
    public int GetY()
    {
        return (int)transform.localPosition.y;
    }
	

}
