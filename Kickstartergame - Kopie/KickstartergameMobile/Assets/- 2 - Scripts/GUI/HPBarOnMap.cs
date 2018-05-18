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
    public Color fullHPColor;
    public Color lowHPColor;
    public Color middleHPColor;
	// Use this for initialization
	void Start () {	
        healthImage = GetComponent<Image>();
	}
    public void SetHealth(int value, int maxValue)
    {
        maxHealth = maxValue;
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
        if (fillAmount >= 0.5f)
        {
            healthImage.color = Color.Lerp(middleHPColor, fullHPColor, MapValues(fillAmount,0.5f,1,0,1));
        }
        else
        {
            healthImage.color = Color.Lerp(lowHPColor, middleHPColor, MapValues(fillAmount, 0f, 0.5f, 0, 1));
        }


    }

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax ){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
