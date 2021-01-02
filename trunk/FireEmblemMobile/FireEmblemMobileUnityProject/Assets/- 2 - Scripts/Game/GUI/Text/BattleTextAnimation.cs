using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class BattleTextAnimation : MonoBehaviour
    {
        public static event Action OnFinished;
        public static event Action OnStarted;
        public TextMeshProUGUI text;
        public Image BackGround;
        private const float FADE_IN_DURATION =  0.25f;
        private const float FADE_OUT_DURATION = 0.25f;
        private const float TEXT_FADE_IN_DURATION = 0.55f;
        private const float TEXT_FADE_OUT_DURATION = 0.55f;

        private void OnEnable()
        {
            OnStarted?.Invoke();
            float posX= text.transform.localPosition.x;
            float posY = text.transform.localPosition.y;
            float posZ = text.transform.localPosition.z;
            RectTransform textRect = text.GetComponent<RectTransform>();
            text.transform.localPosition = new Vector3(posX - ((Screen.width/2) + (textRect.sizeDelta.x / 2)), posY, posZ);
            BackGround.transform.localScale = new Vector3(1, 0, 1);

            LeanTween.scaleY(BackGround.gameObject, 1, FADE_IN_DURATION).setEaseOutQuad().setOnComplete(
                () =>
                    LeanTween.moveLocalX(text.gameObject, posX, TEXT_FADE_IN_DURATION).setEaseOutQuad()
            );
           
        }

       
         
    }
}