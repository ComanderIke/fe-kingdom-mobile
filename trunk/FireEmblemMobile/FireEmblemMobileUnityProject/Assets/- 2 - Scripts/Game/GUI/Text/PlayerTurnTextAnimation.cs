using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Text
{
    public class PlayerTurnTextAnimation : MonoBehaviour, IPhaseRenderer
    {

        private Canvas canvas;
        public TextMeshProUGUI text;
        public Image BackGround;
        public Material []textMaterials;
        public string[] phaseTexts;
        private const float FADE_IN_DURATION =  0.20f;
        private const float FADE_OUT_DURATION = 0.20f;
        private const float TEXT_FADE_IN_DURATION = 0.50f;
        private const float TEXT_FADE_OUT_DURATION = 0.50f;

        void Awake()
        {
            canvas = GetComponent<Canvas>();
        }
        public void Show(FactionId playerId, Action OnFinished)
        {
           // Debug.Log("Show PLayer Text");
            canvas.enabled = true;

           // text.material = textMaterials[playerId];
            text.fontMaterial = textMaterials[(int)playerId];

            text.SetText(phaseTexts[(int)playerId]);
      
            RectTransform textRect = text.GetComponent<RectTransform>();
            float posX = 0;
            float posY = 0;
            
            textRect.anchoredPosition = new Vector3(posX - ((Screen.width/2) + (textRect.sizeDelta.x / 2)), posY);
            BackGround.transform.localScale = new Vector3(1, 0, 1);

            LeanTween.scaleY(BackGround.gameObject, 1, FADE_IN_DURATION).setEaseOutQuad().setOnComplete(
                () =>
                    LeanTween.moveLocalX(text.gameObject, posX, TEXT_FADE_IN_DURATION).setEaseOutQuad().setOnComplete(
                        () =>
                            LeanTween.moveLocalX(text.gameObject, posX + (Screen.width / 2) + (textRect.sizeDelta.x / 2), TEXT_FADE_OUT_DURATION).setEaseInQuad().setOnComplete(
                                () => LeanTween.scaleY(BackGround.gameObject, 0, FADE_OUT_DURATION).setEaseInQuad().setOnComplete(
                                    () =>
                                    {
                                        //Debug.Log("Finish Show PLayer Text");
                                        OnFinished?.Invoke();Hide();
                                    }
                                      )
                            )
                    )
            );
           
        }

        private void Hide()
        {
            canvas.enabled = false;
        }
        
         
    }
}