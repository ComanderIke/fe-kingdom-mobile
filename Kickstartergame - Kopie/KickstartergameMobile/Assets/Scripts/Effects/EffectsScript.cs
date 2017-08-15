using UnityEngine;
using System.Collections;

public class EffectsScript : MonoBehaviour {

    public GameObject[] BloodEffects;
    public GameObject Blood;
    public GameObject LargeBlood;
    public GameObject Wait;
    public GameObject Shield;
	public GameObject ShieldBreak;
    public GameObject HealOnChar;
    public GameObject BackStabOnChar;
    public GameObject StunOnChar;
    public GameObject ClashOnChar;
    public GameObject ArmorUp;
    public GameObject BattleCry;
    public GameObject IceBlock;
    public GameObject ArcaneBolt;
    public GameObject LightningBolt;
    public GameObject LevelUpEffect;
    public GameObject PointOfInterest;
    public GameObject TerrainHeal;
    public GameObject TrailAroundCharacter;
    public GameObject TerrainEffect1;
    public GameObject TerrainEffect2;
    public GameObject TerrainEffect3;
    public GameObject LevelUpOrbs;
    public GameObject whiteOrb;
    public GameObject blackOrb;
    public GameObject redOrb;
    public GameObject purpleOrb;
    public GameObject greenOrb;
    public GameObject blueOrb;
    public GameObject newLevelUpEffect;

    public GameObject GetRandomBloodEffect()
    {
        int rng = Random.Range(0, BloodEffects.Length);
        return BloodEffects[rng];
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
