using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.GUI
{
    public class UIAnimatedCountingText :MonoBehaviour
    {
        [SerializeField]TextMeshProUGUI text;
        [SerializeField] private int countFPS= 30;
        [SerializeField] private float duration = 1.0f;
        
        private Coroutine countingCoroutine;
        public void SetText(string text)
        {
            this.text.text = "" + (text);
        }

        public void SetTextCounting(int startValue,int newValue)
        {
            if (countingCoroutine != null)
            {
                StopCoroutine(countingCoroutine);
            }
            countingCoroutine = StartCoroutine(CountText(startValue, newValue));
        }
        private IEnumerator CountText(int startValue, int newValue)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / countFPS);
            int previousValue = startValue;
            int stepAmount;
            if (newValue - previousValue < 0)
            {
                stepAmount = Mathf.FloorToInt((newValue - previousValue) / (countFPS * duration));
            }
            else
            {
                stepAmount = Mathf.CeilToInt((newValue - previousValue) / (countFPS * duration));
            }

            if (previousValue < newValue)
            {
                while (previousValue < newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue > newValue)
                    {
                        previousValue = newValue;
                    }
                    text.SetText(previousValue.ToString());
                    yield return Wait;
                }
            }
            else
            {
                while (previousValue > newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue < newValue)
                    {
                        previousValue = newValue;
                    }
                    text.SetText(previousValue.ToString());
                    yield return Wait;
                }
            }
            
        }

    }
}