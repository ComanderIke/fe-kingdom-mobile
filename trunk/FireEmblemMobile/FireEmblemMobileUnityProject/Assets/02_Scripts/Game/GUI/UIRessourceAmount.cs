using System.Collections;
using System.Collections.Generic;
using Game.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace LostGrace
{
    public class UIRessourceAmount : MonoBehaviour
    {
        [SerializeField] private UIAnimatedCountingText countingText;
        [FormerlySerializedAs("goldEarnedEffect")] [SerializeField] private ParticleSystem earnedEffect;
        [FormerlySerializedAs("goldLostEffect")] [SerializeField] private ParticleSystem lostEffect;
        
        private int amount;
        private bool init = true;

        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                if (init)
                {
                    amount = value;
                    init = false;
                    countingText.SetText(amount.ToString());
                    return;
                }
                if (value > amount)
                {
                    earnedEffect.Play();
                }
                else if (value < amount)
                {
                    lostEffect.Play();
                }
                countingText.SetTextCounting(amount, value);
                amount = value;
            }
        }

       
        

       

        
    }
}
