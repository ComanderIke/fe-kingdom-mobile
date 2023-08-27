using System;
using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIMoralityBar : MonoBehaviour
    {
        [SerializeField] private Slider moralitySlider;
        [SerializeField] private Color goodColor;
        [SerializeField] private Color neutralColor;
        [SerializeField] private Color evilColor;

        private Morality morality;
        public void Show(Morality morality)
        {
            this.morality = morality;
            morality.OnMoralityChanged -= UpdateUI;
            morality.OnMoralityChanged += UpdateUI;
            UpdateUI(morality.GetCurrentMoralityValue());

        }

        private void OnDisable()
        {
           morality.OnMoralityChanged -= UpdateUI;
        }

        private void UpdateUI(int morality)
        {
            moralitySlider.value = morality;
        }
    }
}
