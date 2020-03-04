using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class StatsBarOnMap : MonoBehaviour
    {
        public int CurrentHealth;
        public int MaxHealth;
        private float fillAmount = 1;
        private float currentValue;
        public float HealthSpeed;
        public static bool Active = true;
        private Image healthImage;
        public Color FullHpColor;
        public Color LowHpColor;
        public Color MiddleHpColor;

        private void Start()
        {
            healthImage = GetComponent<Image>();
        }

        public void SetHealth(int value, int maxValue)
        {
            MaxHealth = maxValue;
            CurrentHealth = value;
        }

        private void Update()
        {
            //TODO Optimize
            if (CurrentHealth != 0)
            {
                currentValue = MapValues(CurrentHealth, 0f, MaxHealth, 0f, 1f);
                fillAmount = Mathf.Lerp(fillAmount, currentValue, Time.deltaTime * HealthSpeed);
                gameObject.transform.localScale = new Vector3(fillAmount, 1, 1);
            }
            else
            {
                fillAmount = 0;
                healthImage.fillAmount = fillAmount;
            }

            healthImage.color = fillAmount >= 0.5f ? Color.Lerp(MiddleHpColor, FullHpColor, MapValues(fillAmount, 0.5f, 1, 0, 1)) : Color.Lerp(LowHpColor, MiddleHpColor, MapValues(fillAmount, 0f, 0.5f, 0, 1));
        }

        private static float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
    }
}