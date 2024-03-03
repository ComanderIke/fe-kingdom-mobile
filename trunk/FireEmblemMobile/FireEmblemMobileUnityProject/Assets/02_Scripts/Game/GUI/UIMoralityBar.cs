using System;
using Game.EncounterAreas.Model;
using Game.GUI.ToolTips;
using Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI
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
        [SerializeField] private ParticleSystem fillParticles;
        [SerializeField]private string GoodAnimationTags;
        [SerializeField]private string EvilAnimationTags;

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
            if(morality!=null)
                morality.OnMoralityChanged -= UpdateUI;
        }

        public void Clicked()
        {
            ToolTipSystem.ShowMorality(morality.GetCurrentMoralityValue());
        }
        private void UpdateUI(float morality, float addedMorality)
        {
            fillParticles.Play();
       
            valueChangedText.text = addedMorality>0?"+"+addedMorality:""+addedMorality;
            valueChangedText.gameObject.SetActive(true);

             evilText.SetText(addedMorality<0?EvilAnimationTags+"Evil":"Evil");
             goodText.SetText(addedMorality>0?GoodAnimationTags+"Good":"Good");
            MonoUtility.DelayFunction(()=>valueChangedText.gameObject.SetActive(false), 2.0f);
            morality = morality / 100f;
            float tweenDuration = Math.Max(0.5f,4.0f * Math.Abs(morality));
            float startValue = moralitySlider.value;
            LeanTween.value(gameObject, startValue, morality, tweenDuration).setEaseInOutQuad().setOnUpdate(
                val =>
                {
                    moralitySlider.SetValueWithoutNotify(val);
                }).setOnComplete(()=>
            {
              
              
               
                fillParticles.Stop();
            });
            MonoUtility.DelayFunction(() =>
            {
                 evilText.SetText("<noparse>Evil");
                goodText.SetText("<noparse>Good");
            }, tweenDuration+1);
         
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
                var startColor = goodText.color;
                LeanTween.value(goodText.gameObject, startColor, Color.Lerp(neutralColor, goodColor, morality),
                    tweenDuration).setOnUpdate((Color val) =>
                {
                    goodText.color = val;
                }).setEaseInOutQuad();
                // LeanTween.color(goodText.transform as RectTransform, Color.Lerp(neutralColor, goodColor, morality), tweenDuration)
                //     .setEaseInOutQuad();
                // LeanTween.color(evilText.transform as RectTransform, Color.Lerp(neutralColor, grayColor, morality), tweenDuration)
                //     .setEaseInOutQuad();
                startColor = evilText.color;
                LeanTween.value(evilText.gameObject, startColor, Color.Lerp(neutralColor, grayColor, morality),
                    tweenDuration).setOnUpdate((Color val) =>
                {
                    evilText.color = val;
                }).setEaseInOutQuad();
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
                var startColor = goodText.color;
                LeanTween.value(goodText.gameObject, startColor, Color.Lerp(grayColor, neutralColor, morality + 1),
                    tweenDuration).setOnUpdate((Color val) =>
                {
                    goodText.color = val;
                }).setEaseInOutQuad();
                // LeanTween.color(goodText.transform as RectTransform, Color.Lerp(grayColor, neutralColor, morality + 1), tweenDuration)
                //     .setEaseInOutQuad();
                // LeanTween.color(evilText.transform as RectTransform, Color.Lerp(evilColor, neutralColor, morality+1), tweenDuration)
                //     .setEaseInOutQuad();
                startColor = evilText.color;
                LeanTween.value(evilText.gameObject, startColor, Color.Lerp(evilColor, neutralColor, morality+1),
                    tweenDuration).setOnUpdate((Color val) =>
                {
                    evilText.color = val;
                }).setEaseInOutQuad();
                LeanTween.color(barBackground.transform as RectTransform, Color.Lerp(evilColorBarBackground, neutralColor, morality+1), tweenDuration)
                    .setEaseInOutQuad();
                LeanTween.color(handle.transform as RectTransform, Color.Lerp(evilColorHandle, neutralColor, morality+1), tweenDuration)
                    .setEaseInOutQuad();
            }
        }
    }
}
