using Assets.Scripts.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OrbStats
{
    Attack,
    Speed,
    Accuracy,
    HP,
    Spirit,
    Defense,
    Movement,
    Range,
    SkillPoint
}
public class Orb
{
    public OrbType type;
    public int relativeProb;
    public Dictionary<OrbStats, int> attributeProb;
    

    private Orb(OrbType type, int prob, Dictionary<OrbStats, int> attributeProb)
    {
        this.type = type;
        this.relativeProb = prob;
        this.attributeProb = attributeProb;
    }
    public static Orb getRedOrb(int prob)
    {
        Dictionary<OrbStats, int> dict = new Dictionary<OrbStats, int>();
        dict.Add(OrbStats.Attack, 1);
        dict.Add(OrbStats.Accuracy, 1);
        dict.Add(OrbStats.Speed, 1);
        return new Orb(OrbType.Red, prob, dict);
    }
    public static Orb getBlueOrb(int prob)
    {
        Dictionary<OrbStats, int> dict = new Dictionary<OrbStats, int>();
        dict.Add(OrbStats.Spirit, 1);
        dict.Add(OrbStats.Accuracy, 1);
        dict.Add(OrbStats.Speed, 1);
        return new Orb(OrbType.Blue, prob, dict);
    }
    public static Orb getGreenOrb(int prob)
    {
        Dictionary<OrbStats, int> dict = new Dictionary<OrbStats, int>();
        dict.Add(OrbStats.Speed, 1);
        dict.Add(OrbStats.HP, 1);
        dict.Add(OrbStats.Defense, 1);
        return new Orb(OrbType.Green, prob, dict);
    }
    public static Orb getBlackOrb(int prob)
    {
        Dictionary<OrbStats, int> dict = new Dictionary<OrbStats, int>();
        dict.Add(OrbStats.Spirit, 1);
        dict.Add(OrbStats.HP, 1);
        dict.Add(OrbStats.Defense, 1);
        return new Orb(OrbType.Black, prob, dict);
    }
    public static Orb getPurpleOrb(int prob)
    {
        Dictionary<OrbStats, int> dict = new Dictionary<OrbStats, int>();
        dict.Add(OrbStats.Spirit, 1);
        dict.Add(OrbStats.Attack, 1);
        dict.Add(OrbStats.Accuracy, 1);
        return new Orb(OrbType.Purple, prob, dict);
    }
    public static Orb getWhiteOrb(int prob)
    {
        Dictionary<OrbStats, int> dict = new Dictionary<OrbStats, int>();
        dict.Add(OrbStats.HP, 1);
        dict.Add(OrbStats.Attack, 1);
        dict.Add(OrbStats.Defense, 1);
        return new Orb(OrbType.White, prob, dict);
    }
}
public enum OrbType
{
    Red,
    Green,
    Blue,
    Purple,
    Black,
    White
}
public class OrbScript : MonoBehaviour {

    Orb orb;
    List<OrbStats> attributes;
    string text = "";
    Character character;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        CameraMovement.locked = false;
        CharacterScript.lockInput = false;
        CameraMovement.Desaturate(0.5f, 1.25f);
        Debug.Log("Orb Clicked" + orb.type);
        float delay = 0;
        foreach(OrbStats os in attributes)
        {
            switch (os)
            {
                case OrbStats.Accuracy: character.AddAttribute(StatAttribute.Accuracy,1,delay); break;
                case OrbStats.Attack: character.AddAttribute(StatAttribute.Attack, 1, delay); break;
                case OrbStats.Defense: character.AddAttribute(StatAttribute.Defense, 1, delay); break;
                case OrbStats.HP: character.AddAttribute(StatAttribute.HP, 2, delay); break;
                case OrbStats.Speed: character.AddAttribute(StatAttribute.Speed, 1, delay); break;
                //case OrbStats.Movement: character.stats.maxHP += 1; break;
                case OrbStats.Spirit: character.AddAttribute(StatAttribute.Spirit, 1, delay); break;
                //case OrbStats.Range: character.stats.maxHP += 1; break;
            }
            delay += 1.2f;
        }
        //  foreach(ParticleSystem ps in CharacterScript.LevelUpEffect.GetComponentsInChildren<ParticleSystem>())
        // {

        FindObjectOfType<CharacterScript>().FadeOutLevelUpEffect();
        //}
        // CharacterScript.LevelUpEffect.set
        // GameObject.Destroy(CharacterScript.LevelUpEffect);
        GameObject.Destroy(FindObjectOfType<LevelUpOrbs>().gameObject);

    }
   
    private void OnMouseEnter()
    {
        smokeParticle.SetActive(true);
        orbGO.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        GetComponentInChildren<Text>().text = text;
        GetComponentInChildren<Text>().enabled = true;
        GetComponentInChildren<Text>().color = Color.white;
    }
    private void OnMouseExit()
    {
        orbGO.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        smokeParticle.SetActive(false);
        GetComponentInChildren<Text>().enabled = false;
    }
    GameObject orbGO;
    GameObject smokeParticle;
    public void SetOrb(Character character, Orb orb)
    {
        this.character = character;
        this.orb = orb;
        Color c = Color.black;
        EffectsScript es = FindObjectOfType<EffectsScript>();
        switch (orb.type)
        {
            case OrbType.Red: c = FindObjectOfType<MaterialScript>().redOrb;orbGO = GameObject.Instantiate(es.redOrb, this.transform) as GameObject;
                break;
            case OrbType.Green: c = FindObjectOfType<MaterialScript>().greenOrb; orbGO = GameObject.Instantiate(es.greenOrb, this.transform);  break;
            case OrbType.Blue: c = FindObjectOfType<MaterialScript>().blueOrb; orbGO = GameObject.Instantiate(es.blueOrb, this.transform); break;
            case OrbType.Purple: c = FindObjectOfType<MaterialScript>().purpleOrb; orbGO = GameObject.Instantiate(es.purpleOrb, this.transform); break;
            case OrbType.Black: c = FindObjectOfType<MaterialScript>().blackOrb; orbGO = GameObject.Instantiate(es.blackOrb, this.transform); break;
            case OrbType.White: c = FindObjectOfType<MaterialScript>().whiteOrb; orbGO = GameObject.Instantiate(es.whiteOrb, this.transform); break;

        }
        orbGO.transform.localScale = new Vector3(1.2f, 1.2f,1.2f);
        orbGO.transform.localPosition = new Vector3();
        smokeParticle = orbGO.GetComponentsInChildren<ParticleSystem>()[5].gameObject;
        smokeParticle.SetActive(false);
        //this.GetComponent<MeshRenderer>().material.color = c;
        int totalSum = 0;
        foreach (KeyValuePair<OrbStats, int> entry in orb.attributeProb)
        {
            totalSum+=entry.Value;
        }
        attributes = new List<OrbStats>();
        for (int j = 0; j < 2; j++)
        {
            OrbStats stats = OrbStats.HP;

            int index = Random.Range(1, totalSum+1);
            int sum = 0;
            int i = 0;
            foreach (KeyValuePair<OrbStats, int> entry in orb.attributeProb)
            {
                if (sum < index)
                {
                    sum = sum + entry.Value;
                    stats = entry.Key;
                }
                else
                {
                    break;
                }
            }
            attributes.Add(stats);
        }
        List<OrbStats> contained = new List<OrbStats> ();
        foreach (OrbStats os in attributes)
        {
            if (!contained.Contains(os))
            {
                int count = 0;
                foreach (OrbStats os2 in attributes)
                {
                    if (os2 == os)
                        count++;
                    if (count >= 2)
                    {
                        contained.Add(os);
                    }
                }
                if(os==OrbStats.HP)
                    text += os + " + " + count*2 + "\n";
                else
                    text += os + " + " + count + "\n";
            }
        }
        text = text.Substring(0, text.Length - 1);
        
    }
}
