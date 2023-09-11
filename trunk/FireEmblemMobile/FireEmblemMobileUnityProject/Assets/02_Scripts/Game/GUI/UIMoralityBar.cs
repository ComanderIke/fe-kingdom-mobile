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
        [SerializeField] private Color grayColor;
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
        [SerializeField] private TextMeshProUGUI valueChangedText;

        private Morality morality;
        public void Show(Morality morality)
        {
            this.morality = morality;
            morality.OnMoralityChanged -= UpdateUI;
            morality.OnMoralityChanged += UpdateUI;
            UpdateUI(morality.GetCurrentMoralityValue(),0);

        }

        public void onSliderValueChanged()
        {
            UpdateUI(moralitySlider.value,0);
        }

        private void OnDisable()
        {
           morality.OnMoralityChanged -= UpdateUI;
        }

        private void UpdateUI(float morality, float addedMorality)
        {
            Debug.Log("Morality: "+morality+" Added: "+addedMorality);
            valueChangedText.text = addedMorality>0?"+"+addedMorality:""+addedMorality;
            valueChangedText.gameObject.SetActive(true);
            MonoUtility.DelayFunction(()=>valueChangedText.gameObject.SetActive(false), 2.0f);
            morality = morality / 100f;
            float tweenDuration = Math.Max(0.5f,4.0f * Math.Abs(morality));
            float startValue = moralitySlider.value;
            LeanTween.value(gameObject, startValue, morality, tweenDuration).setEaseInOutQuad().setOnUpdate(
                val =>
                {
                    moralitySlider.SetValueWithoutNotify(val);
                });
         
            if (morality > 0)
            {
                // backgroundEvil.alpha = 0;
                // backgroundNeutral.alpha = 1 - morality;
                // backgroundGood.alpha = morality;
                // goodText.color = Color.Lerp(neutralColor, goodColor, morality);
                // evilText.color = Color.Lerp(neutralColor, grayColor, morality);
                // barBackground.color=Color.Lerp(neutralColor, goodColorBarBackground, morality);
                // handle.color=Color.Lerp(neutralColor, goodColorHandle, morality);
                LeanTween.alphaCanvas(backgroundEvil, 0, tweenDuration) .setEaseInOutQuad();
                LeanTween.alphaCanvas(backgroundNeutral, 1 - morality, tweenDuration) .setEaseInOutQuad();
                LeanTween.alphaCanvas(backgroundGood, morality, tweenDuration) .setEaseInOutQuad();
                LeanTween.color(goodText.transform as RectTransform, Color.Lerp(neutralColor, goodColor, morality), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(evilText.transform as RectTransform, Color.Lerp(neutralColor, grayColor, morality), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(barBackground.transform as RectTransform, Color.Lerp(neutralColor, goodColorBarBackground, morality), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(handle.transform as RectTransform, Color.Lerp(neutralColor, goodColorHandle, morality), tweenDuration)
                    .setEaseInOutQuad();
            }
            else
            {
                //evilText.color = Color.Lerp(evilColor, neutralColor, morality+1);
               // goodText.color = Color.Lerp(grayColor, neutralColor, morality + 1);
                // backgroundEvil.alpha = morality*-1;
                // backgroundNeutral.alpha =  morality+1;
                // backgroundGood.alpha = 0;
                //barBackground.color=Color.Lerp(evilColorBarBackground, neutralColor, morality+1);
                //handle.color=Color.Lerp(evilColorHandle, neutralColor, morality+1);
                LeanTween.alphaCanvas(backgroundEvil, morality*-1, tweenDuration) .setEaseInOutQuad();
                LeanTween.alphaCanvas(backgroundNeutral, morality+1, tweenDuration) .setEaseInOutQuad();
                LeanTween.alphaCanvas(backgroundGood, 0, tweenDuration) .setEaseInOutQuad();
                LeanTween.color(goodText.transform as RectTransform, Color.Lerp(grayColor, neutralColor, morality + 1), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(evilText.transform as RectTransform, Color.Lerp(evilColor, neutralColor, morality+1), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(barBackground.transform as RectTransform, Color.Lerp(evilColorBarBackground, neutralColor, morality+1), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(handle.transform as RectTransform, Color.Lerp(evilColorHandle, neutralColor, morality+1), tweenDuration)
                    .setEaseInOutQuad();
            }
        }
    }
}
