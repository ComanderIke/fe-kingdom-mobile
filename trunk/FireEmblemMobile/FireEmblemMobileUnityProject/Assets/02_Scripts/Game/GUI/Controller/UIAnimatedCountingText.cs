using System;
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
        [SerializeField] private string prefix = "";
        private Coroutine countingCoroutine;
        private int startValue=0;
        private int newValue=30;
        public void SetText(string text)
        {
            this.text.text = prefix+"" + (text);
        }

        public int GetCurrentAmount()
        {
           // Debug.Log(text.text);
            return Int32.Parse(text.text.Remove(0,prefix.Length));
        }
        public void StartCounting()
        {
            countingCoroutine = StartCoroutine(CountText(startValue, newValue));
        }

        public void SetTextCounting(int startValue, int newValue, bool startCounting = true)
        {
            if (countingCoroutine != null)
            {
                StopCoroutine(countingCoroutine);
            }
            if(startCounting)
                countingCoroutine = StartCoroutine(CountText(startValue, newValue));
            else
            {
                this.startValue = startValue;
                this.newValue = newValue;
            }
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
                    SetText(previousValue.ToString());
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
                    SetText(previousValue.ToString());
                    yield return Wait;
                }
            }
            
        }

    }
}