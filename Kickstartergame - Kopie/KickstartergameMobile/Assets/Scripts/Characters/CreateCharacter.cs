using UnityEngine;
using System.Collections;
using Assets.Scripts.Characters.Classes;

public class CreateCharacter : MonoBehaviour {

	public GameObject melee;
    public GameObject rogue;
    public GameObject mage;
    public GameObject archer;
    public GameObject priest;
    public GameObject hellebardier;
	public GameObject hellebardierEnemy;
	public GameObject mageEnemy;
	public GameObject archerEnemy;
	public GameObject tankEnemy;
    public GameObject meleeWeaponPosition;
    public GameObject rangedWeaponPosition;
    public GameObject mageWeaponPosition;
    Color[] colors;
    Color[] skinColors;
    // Use this for initialization
    void Start () {
		

	}

    public void placeCharacter(int PlayerNumber, Character c ,int x, float y, int z)
    {
        GameObject o = null;
        string player = "Player" + (PlayerNumber+1);
        int skinindex = 0;
        if (c.characterClassType == CharacterClassType.SwordFighter)
        {
			if(PlayerNumber==0)
				o = Instantiate<GameObject>(melee);
			else
				o = Instantiate<GameObject>(tankEnemy);
            skinindex = 4;
            //GameObject weapon = Instantiate<GameObject>(c.GetWeapon().go);
            //weapon.transform.SetParent(o.GetComponentInChildren<WeaponTransform>().transform);
            //weapon.transform.localRotation = c.GetWeapon().go.transform.localRotation;//Quaternion.Euler(2, 190, 290);
            //weapon.transform.localScale = c.GetWeapon().go.transform.localScale;//= new Vector3(0.006f, 0.006f, 0.006f) 
            //weapon.transform.localPosition = c.GetWeapon().go.transform.localPosition;
        }
        if (c.characterClassType == CharacterClassType.Hellebardier)
        {
			if(PlayerNumber==0)
            o = Instantiate<GameObject>(hellebardier);
			else
				o = Instantiate<GameObject>(hellebardierEnemy);
            skinindex = 4;
            //GameObject weapon = Instantiate<GameObject>(c.GetWeapon().go);
            //weapon.transform.SetParent(o.GetComponentInChildren<WeaponTransform>().transform);
            //weapon.transform.localRotation = c.GetWeapon().go.transform.localRotation;//Quaternion.Euler(2, 190, 290);
            //weapon.transform.localScale = c.GetWeapon().go.transform.localScale;//= new Vector3(0.006f, 0.006f, 0.006f) 
            //weapon.transform.localPosition = c.GetWeapon().go.transform.localPosition;
        }
        if (c.characterClassType == CharacterClassType.Rogue)
        {
            o = Instantiate<GameObject>(rogue);
            skinindex = 2;
            //GameObject weapon = Instantiate<GameObject>(c.GetWeapon().go);
            //weapon.transform.SetParent(o.GetComponentInChildren<WeaponTransform>().transform);
            //weapon.transform.localRotation = c.GetWeapon().go.transform.localRotation; //Quaternion.Euler(50, 140, 100);
            //weapon.transform.localScale = c.GetWeapon().go.transform.localScale;
            //weapon.transform.localPosition = c.GetWeapon().go.transform.localPosition;// = new Vector3(0,0,0);
        }
        if (c.characterClassType == CharacterClassType.Archer)
        {
			if(PlayerNumber!=1)
				o = Instantiate<GameObject>(archer);
			else
            o = Instantiate<GameObject>(archerEnemy);
            skinindex = 2;
            //GameObject weapon = Instantiate<GameObject>(c.GetWeapon().go);
            //weapon.transform.SetParent(o.GetComponentInChildren<WeaponTransform>().transform);
            //weapon.transform.localRotation = c.GetWeapon().go.transform.localRotation; //Quaternion.Euler(50, 140, 100);
            //weapon.transform.localScale = c.GetWeapon().go.transform.localScale;
            //weapon.transform.localPosition = c.GetWeapon().go.transform.localPosition;// = new Vector3(0,0,0);
        }
        if (c.characterClassType == CharacterClassType.Mage) {
			if(PlayerNumber==0)
				o = Instantiate<GameObject>(mage);
			else
            o = Instantiate<GameObject>(mageEnemy);
            skinindex = 8;
        }
        if (c.characterClassType == CharacterClassType.Priest)
        {
            o = Instantiate<GameObject>(priest);
            skinindex = 8;
        }
        o.name = c.name;
		if (c.name == "Leila")
			o.name = "Rosali";
		if (c.name == "Hector") {
			o.name = "Siegfried";
		}
        o.tag = "Player";
       
       
        o.transform.parent = GameObject.Find(player).GetComponentInChildren<ManageCharacters>().gameObject.transform;
        CharacterScript characterScript = o.GetComponentInChildren<CharacterScript>();
        characterScript.setCharacter(c);
        characterScript.getCharacter().gameObject = o;
        o.transform.localPosition = new Vector3(x, y, z);
        o.transform.Rotate(new Vector3(0, 1, 0), 180);
        c.InstantiateWeapon();
        skinColors = new Color[21] { Color.white,
            ConvertColor(237, 219, 205), ConvertColor(215,189,169), ConvertColor(249,237,228), ConvertColor(218,137,094), ConvertColor(153,128,112),
            ConvertColor(232,208,195), ConvertColor(227,191,167), ConvertColor(235,221,214), ConvertColor(108,091,081), ConvertColor(066,055,049),
            ConvertColor(210,197,189), ConvertColor(189,176,168), ConvertColor(220,208,201), ConvertColor(243,196,165), ConvertColor(151,143,139),
            ConvertColor(097,069,053), ConvertColor(067,056,050), ConvertColor(243,194,189), ConvertColor(174,100,073), ConvertColor(078,038,036) };
        colors = new Color[21] { Color.white,
            ConvertColor(233,173,059), ConvertColor(217,027,027), ConvertColor(244,243,240), ConvertColor(152,018,017), ConvertColor(152,019,199),
            ConvertColor(029,060,026), ConvertColor(029,043,028), ConvertColor(140,122,082), ConvertColor(077,071,059), ConvertColor(039,028,008),
            ConvertColor(030,044,107), ConvertColor(009,022,077), ConvertColor(151,154,171), ConvertColor(048,051,068), ConvertColor(108,110,123),
            ConvertColor(026,176,030), ConvertColor(255,131,010), ConvertColor(184,215,231), ConvertColor(162,147,238), ConvertColor(027,073,241) };
        c.gameObject.GetComponent<CharacterScript>().SetHPBar();
        GameObject.Find("Player1").GetComponentInChildren<AssignColorToCharacters>().ApplyColor();
        GameObject.Find("Player2").GetComponentInChildren<AssignColorToCharacters>().ApplyColor();
    }
    Color ConvertColor(int r, int g, int b)
    {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
