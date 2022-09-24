using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace LostGrace
{
    public class UIGold : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI goldAmountText;

        [SerializeField] private int countFPS= 30;

        [SerializeField] private float duration = 1.0f;

        [SerializeField] private ParticleSystem goldEarnedEffect;
        [SerializeField] private ParticleSystem goldLostEffect;
        private int goldAmount;
        private bool init = true;

        public int GoldAmount
        {
            set
            {
                if (init)
                {
                    goldAmount = value;
                    init = false;
                    goldAmountText.SetText(goldAmount.ToString());
                    return;
                }
                if (value > goldAmount)
                {
                    goldEarnedEffect.Play();
                }
                else if (value < goldAmount)
                {
                    goldLostEffect.Play();
                }
                UpdateText(value);
                goldAmount = value;
            }
        }

        private Coroutine countingCoroutine;
        private void UpdateText(int newValue)
        {
            if (countingCoroutine != null)
            {
                StopCoroutine(countingCoroutine);
            }
            countingCoroutine = StartCoroutine(CountText(newValue));
        }

        private IEnumerator CountText(int newValue)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / countFPS);
            int previousValue = goldAmount;
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
                    goldAmountText.SetText(previousValue.ToString());
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
                    goldAmountText.SetText(previousValue.ToString());
                    yield return Wait;
                }
            }
            
        }

        
    }
}
