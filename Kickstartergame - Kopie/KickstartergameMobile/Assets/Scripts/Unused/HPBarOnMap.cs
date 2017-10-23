using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Characters;

public class HPBarOnMap : MonoBehaviour {
    public int currentHealth;
    public int maxHealth;
	private float fillAmount = 1;
	private float currentValue;
	public float healthSpeed;
	public static bool active = true;
    private Image healthImage;
	// Use this for initialization
	void Start () {	
        healthImage = GetComponent<Image>();
	}
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
    }
    public void SetCurrentHealth(int value)
    {
        currentHealth = value;
    }
    void Update () {
		//TODO Optimize
        if (currentHealth != 0)
        {
            currentValue = MapValues(currentHealth, 0f, maxHealth, 0f, 1f);

            fillAmount = Mathf.Lerp(fillAmount, currentValue, Time.deltaTime * healthSpeed);
            this.gameObject.transform.localScale = new Vector3(fillAmount, 1, 1);
        }
        else
        {
            fillAmount = 0;
            healthImage.fillAmount = fillAmount;
        }
			
	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax ){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
