using System;
using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class UIMoralityBar : MonoBehaviour
    {
        [SerializeField] private Slider moralitySlider;
        [SerializeField] private Color goodColor;
        [SerializeField] private Color goodColorHandle;
        [SerializeField] private Color goodColorBarBackground;
        [SerializeField] private Color neutralColor;
        [SerializeField] private Color evilColor;
        [SerializeField] private Color evilColorHandle;
        [SerializeField] private Color evilColorBarBackground;
        [SerializeField] private CanvasGroup backgroundNeutral;
        [SerializeField] private CanvasGroup backgroundGood;
        [SerializeField] private CanvasGroup backgroundEvil;
        [SerializeField] private TextMeshProUGUI evilText;
        [SerializeField] private TextMeshProUGUI goodText;
        [SerializeField] private Image barBackground;
        [SerializeField] private Image handle;

        private Morality morality;
        public void Show(Morality morality)
        {
            this.morality = morality;
            morality.OnMoralityChanged -= UpdateUI;
            morality.OnMoralityChanged += UpdateUI;
            UpdateUI(morality.GetCurrentMoralityValue());

        }

        public void onSliderValueChanged()
        {
            UpdateUI(moralitySlider.value);
        }

        private void OnDisable()
        {
           morality.OnMoralityChanged -= UpdateUI;
        }

        private void UpdateUI(float morality)
        {
            moralitySlider.value = morality;
            if (morality > 0)
            {
                backgroundEvil.alpha = 0;
                backgroundNeutral.alpha = 1 - morality;
                backgroundGood.alpha = morality;
                goodText.color = Color.Lerp(neutralColor, goodColor, morality);
                evilText.color = neutralColor;
                barBackground.color=Color.Lerp(neutralColor, goodColorBarBackground, morality);
                handle.color=Color.Lerp(neutralColor, goodColorHandle, morality);
            }
            else
            {
                evilText.color = Color.Lerp(evilColor, neutralColor, morality+1);
                goodText.color = neutralColor;
                backgroundEvil.alpha = morality*-1;
                backgroundNeutral.alpha =  morality+1;
                backgroundGood.alpha = 0;
                barBackground.color=Color.Lerp(evilColorBarBackground, neutralColor, morality+1);
                handle.color=Color.Lerp(evilColorHandle, neutralColor, morality+1);
            }
        }
    }
}
