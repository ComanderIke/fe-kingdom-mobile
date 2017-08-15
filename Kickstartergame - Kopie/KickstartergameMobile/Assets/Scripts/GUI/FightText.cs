using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Characters.Classes;
using AssemblyCSharp;

[System.Serializable]
public enum FightTextType{
	Damage,
	Heal,
	Critical,
    Blocked,
	Missed
}
[System.Serializable]
public class DamageText
{
    public FightTextType ftt;
    public Vector3 position;
    public string text;
    public DamageText(Vector3 position, string text, FightTextType ftt)
    {
        this.ftt = ftt;
        this.text = text;
        this.position = position;
    }
}
public class FightText : MonoBehaviour {
	Text text;
	GameObject animationObject;
    public AudioClip mageAttack;
    public AudioClip ninjaAttack;
    public AudioClip WarriorAttack;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		//animationObject = GameObject.Find ("RessourceScript").GetComponent<SkillAnimationScript> ().Slash;
       
    }
	const float SCALE = 0.01f;
	// Update is called once per frame
	bool isActive =false;
	float time = 0;
	float alpha = 1;
	float scale =0;
    bool init = true;
	int fontsize=0;
	void Update () {
		if (isActive) {
            if (list.Count != 0)
            {
                DamageText dt = list[0];
                if (init)
                {
                    init = false;
                    Vector3 position = list[0].position;//GameObject.Find("UICamera").GetComponent<Camera>().WorldToScreenPoint(list[0].position);
                    BloodPosition pos = defender.gameObject.GetComponentInChildren<BloodPosition>();
                    animationObject.transform.position = new Vector3(pos.transform.position.x, pos.transform.position.y, pos.transform.position.z);
                    GameObject.Instantiate(animationObject);
                    string str = list[0].text;
                    FightTextType type = list[0].ftt;
                    text.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
                    transform.position = new Vector3(position.x, position.y, position.z);
                    scale = text.transform.localScale.x;
                    Text t = GetComponent<Text>();
                    t.enabled = true;
                    t.text = str;
                    if (type == FightTextType.Critical)
                    {
                        t.color = Color.yellow;
                        if (charclasstype == CharacterClassType.Mage)
                            GetComponent<AudioSource>().PlayOneShot(mageAttack);
                        else if (charclasstype == CharacterClassType.Rogue)
                                GetComponent<AudioSource>().PlayOneShot(ninjaAttack);
                        else if (charclasstype == CharacterClassType.SwordFighter)
                            GetComponent<AudioSource>().PlayOneShot(WarriorAttack);
                    }
                    else if (type == FightTextType.Missed)
                    {
                        t.color = Color.green;
						t.fontSize = 26;
                    }
                    else if (type == FightTextType.Blocked)
                    {
						t.color = new Color(1,184.0f/255.0f,0);
						t.fontSize = 26;
                    }
                    else if (type == FightTextType.Heal)
                    {
                        t.color = Color.green;
                    }
                    else
                    {
                        t.color = Color.red;
						t.fontSize = 40;
                        if (charclasstype == CharacterClassType.Mage)
                            GetComponent<AudioSource>().PlayOneShot(mageAttack);
                        else if (charclasstype == CharacterClassType.Rogue)
                            GetComponent<AudioSource>().PlayOneShot(ninjaAttack);
                        else if (charclasstype == CharacterClassType.SwordFighter)
                            GetComponent<AudioSource>().PlayOneShot(WarriorAttack);
                    }
                }
                time += Time.deltaTime;
                if (time > 0.5f)
                {
                    alpha -= 2.0f * Time.deltaTime;
                    scale -= 0.003f * Time.deltaTime;
                    text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                    transform.localScale = new Vector3(scale, scale, scale);
                }
                if (time > 1.2f)
                {
                    list.Remove(dt);
                    init = true;
                    text.enabled = false;
                    time = 0;
                    alpha = 1;
                    scale = 0;
                }
            }
            else
            {
                isActive = false;
            }
		}
	}
	public void OnAttackHit(){
	}

    List<DamageText> list = new List<DamageText>();
    CharacterClassType charclasstype;
    private Character defender;
    public void setText(Vector3 position, string str, FightTextType type, Character Attacker, Character Defender){
		if (Attacker!=null&&Attacker.characterClassType == CharacterClassType.Mage) {
			if (Attacker.EquipedWeapon.Name == "Ignis") {
				animationObject = GameObject.Find ("RessourceScript").GetComponent<AttackEffects> ().ignis;
			}
			if (Attacker.EquipedWeapon.Name == "Ventus") {
				animationObject = GameObject.Find ("RessourceScript").GetComponent<AttackEffects> ().ventus;
			}
			if (Attacker.EquipedWeapon.Name == "ThanatosBreath") {
				animationObject = GameObject.Find ("RessourceScript").GetComponent<AttackEffects> ().thanatosBreath;
			}
		}
        else
        {
			animationObject = GameObject.Find("RessourceScript").GetComponent<AttackEffects>().sparks;
        }
        DamageText dt = new DamageText(position,str, type );
        isActive = true;
		if (Attacker != null)
			charclasstype = Attacker.characterClassType;
        defender = Defender;
        list.Add(dt);
		
	}
}
