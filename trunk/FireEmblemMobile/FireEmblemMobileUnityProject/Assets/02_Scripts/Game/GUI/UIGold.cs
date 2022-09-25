using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace LostGrace
{
    public class UIGold : MonoBehaviour
    {
        [SerializeField] private UIAnimatedCountingText countingText;
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
                    countingText.SetText(goldAmount.ToString());
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
                countingText.SetTextCounting(goldAmount, value);
                goldAmount = value;
            }
        }

       
        

       

        
    }
}
