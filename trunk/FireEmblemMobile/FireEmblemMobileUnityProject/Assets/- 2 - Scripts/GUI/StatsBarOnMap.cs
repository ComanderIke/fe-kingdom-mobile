using System.Collections;
using Assets.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class StatsBarOnMap : MonoBehaviour
    {
        public const float HP_BAR_OFFSET_DELAY = 0.005f;
        private int currentHealth;
        private int maxHealth;
        private float fillAmount = 1;


        private float healthSpeed = 10f;
        private float healthLoseSpeed = 10f;

        private float currentHpValue = 1;
        private float delayedHpValue = 1;
        private float loseFillAmount = 1;
        public bool dynamicColor = false;
        [SerializeField] private Image hpBar = default;
        [SerializeField] private Image losingHpBar = default;


        public void SetHealth(int value, int maxValue)
        {
            maxHealth = maxValue;
            currentHealth = value;
            currentHpValue = MathUtility.MapValues(currentHealth, 0f, maxHealth, 0f, 1f);

            //Debug.Log("value: "+value +" " +maxValue+ "CV "+currentHpValue);
            StartCoroutine(DelayedHp());
        }

        private void Update()
        {
            //NEW
            //Debug.Log(currentHpValue); 
            fillAmount = Mathf.Lerp(fillAmount, currentHpValue, Time.deltaTime * healthSpeed);
            loseFillAmount = Mathf.Lerp(loseFillAmount, delayedHpValue, Time.deltaTime * healthLoseSpeed);
            hpBar.fillAmount = fillAmount;
            losingHpBar.fillAmount = loseFillAmount;
        }
        private IEnumerator DelayedHp()
        {
            while (Mathf.Abs(fillAmount - currentHpValue) >= HP_BAR_OFFSET_DELAY)
            {
                yield return null;
            }

            delayedHpValue = MathUtility.MapValues(currentHealth, 0f, maxHealth, 0f, 1f);
        }
        private static float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}