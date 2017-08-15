using UnityEngine;
using System.Collections;

[System.Serializable]
public enum TerrainEffectType
{
    Exp,
    Health
}
public class TerrainEffectPosition : MonoBehaviour {

    public TerrainEffectType effecttype;
	GameObject animation;
	// Use this for initialization
	void Start () {
        GetComponentInChildren<MeshRenderer>().enabled = false;
		Vector3 spawnPos = new Vector3 (this.transform.position.x + 0.5f, this.transform.position.y, this.transform.position.z + 0.5f);
        switch (effecttype)
        {
		
		case TerrainEffectType.Exp: animation =Instantiate(GameObject.Find("RessourceScript").GetComponent<EffectsScript>().TerrainEffect1, spawnPos, Quaternion.identity)as GameObject; break;
		case TerrainEffectType.Health: animation =Instantiate(GameObject.Find("RessourceScript").GetComponent<EffectsScript>().TerrainEffect2, spawnPos, Quaternion.identity)as GameObject; break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Effect(Character c){
		
		switch (effecttype) {
		case TerrainEffectType.Exp:
			GameObject.Find("Message").GetComponent<Message>().Show("Exp + 50");
			c.addExp(50);
			break;
		case TerrainEffectType.Health:
			GameObject.Find("Message").GetComponent<Message>().Show("Restored HP");
			c.HP = c.stats.maxHP;
			break;
		}

		GameObject.Destroy (animation);
	}
}
