using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

	private float currentHealth;
	public Image visualHealth;
	private float currentValue;
	private float maxHealth;
	public float healthSpeed;
	private bool active = false;
	public static Character character;
	Image i;

	// Use this for initialization
	void Start () {
		i =this.gameObject.GetComponent<Image> ();
		i.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			Text t = GetComponentInChildren<Text>();
			t.text = character.HP+ " / " +character.stats.maxHP;
			currentValue = MapValues (character.HP, 0f, character.stats.maxHP, 0f, 1f);
			visualHealth.fillAmount = Mathf.Lerp (visualHealth.fillAmount, currentValue, Time.deltaTime * healthSpeed);
		}
	}
	public void SetActive(Character c){
		character = c;
		active = true;
		i.enabled = true;
	}
	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax ){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
