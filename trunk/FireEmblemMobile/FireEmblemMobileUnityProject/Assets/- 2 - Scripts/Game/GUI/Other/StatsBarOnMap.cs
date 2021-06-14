using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class StatsBarOnMap : IStatBar
    {
        public const float HP_BAR_OFFSET_DELAY = 0.1f;
        private int currentHealth;
        private int maxHealth;
        private float fillAmount = 1;


        private float healthSpeed = 1f;

        private float currentHpValue = 1;
        private float delayedHpValue = 1;
        private float loseFillAmount = 1;
        public bool dynamicColor = false;
        [SerializeField] private Image hpBar = default;
        [SerializeField] private Image losingHpBar = default;


      
        float time = 0;
        float loseTime = 0;
        private void Update()
        {
            //NEW
            //Debug.Log(currentHpValue); 
            time += Time.deltaTime*healthSpeed;
            loseTime += Time.deltaTime * healthSpeed;
            if (fillAmount != currentHpValue|| loseFillAmount != delayedHpValue)
            {
                //Debug.Log("Update Healthbar" + fillAmount+" "+currentHpValue+" "+loseFillAmount+" "+ delayedHpValue+" "+time+" "+ Mathf.Clamp(time, 0f,1f ));
                fillAmount = Mathf.Lerp(fillAmount, currentHpValue, Mathf.Clamp(time,0,1));
                loseFillAmount = Mathf.Lerp(loseFillAmount, delayedHpValue, Mathf.Clamp(loseTime, 0, 1));
                hpBar.fillAmount = fillAmount;
                losingHpBar.fillAmount = loseFillAmount;
            }
        }
        private IEnumerator DelayedHp()
        {
            while (Mathf.Abs(fillAmount - currentHpValue) >= HP_BAR_OFFSET_DELAY)
            {
                yield return null;
            }
            loseTime = 0;
            delayedHpValue = MathUtility.MapValues(currentHealth, 0f, maxHealth, 0f, 1f);
        }

        public override void SetValue(int value, int maxValue)
        {
            maxHealth = maxValue;
            currentHealth = value;
            currentHpValue = MathUtility.MapValues(currentHealth, 0f, maxHealth, 0f, 1f);
            time = 0;
            //Debug.Log("value: "+value +" " +maxValue+ "CV "+currentHpValue);
            if (isActiveAndEnabled)
                StartCoroutine(DelayedHp());
        }
    }
}