using System;
using Game.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class ExpBarController : MonoBehaviour, IExpRenderer
    {
        Vector3 oldPosition;
        public TextMeshProUGUI text;
        public Image fill;
        public GameObject glow;
        int currentExp;
        int addedExp;
        // float textspeed = 3;
        public float AnimateInDuration=0.25f;
        public float AnimateStayDuration = 0.45f;
        public float AnimateOutDuration = 0.2f;
        // Start is called before the first frame update
        public void UpdateValues(int currentExp, int addedExp)
        {
            this.currentExp = currentExp;
            this.addedExp = addedExp;
        }

        public void Play()
        {
            text.text = ""+currentExp;
            LeanTween.alphaCanvas(glow.GetComponent<CanvasGroup>(), 1, 1.0f).setLoopPingPong(1);
            LeanTween.scale(gameObject, new Vector3(1.4f, 1.4f, 1), AnimateInDuration).setDelay(0.25f).setEaseSpring().setOnComplete(ShowTextAnimation);
            oldPosition = transform.localPosition;
        }
  
        void ShowTextAnimation()
        {
            int exp = currentExp;
            float fillAmount = 0;
            float expLeft = 0;
            LeanTween.value(gameObject, 0, addedExp, Math.Max(addedExp / 100f*1.5f, 0.4f)).setEaseOutQuad().setOnUpdate((float val) =>
            {
            
                int intVal = (int)val;
                int expVal = exp + intVal;
                if (expVal > 100)
                {
                    expLeft = expVal - 100;
                    expVal = 100;
                  
                }

                text.text = "" + (expVal);
                fillAmount =  (expVal) /100f;
                fill.fillAmount = fillAmount;

            }).setOnComplete(Hide).setDelay(0.10f);
        }
        void Hide()
        {
            LeanTween.scale(gameObject, new Vector3(1, 1, 1), AnimateOutDuration).setEaseInQuad().setDelay(AnimateStayDuration);
            LeanTween.moveLocal(gameObject, oldPosition, AnimateOutDuration).setEaseInQuad().setDelay(AnimateStayDuration)
                .setOnComplete(() => { AnimationQueue.OnAnimationEnded?.Invoke(); });
        }

        public void SetText(int exp)
        {
            text.text = "" + (exp);
        }

        public void SetFillAmount(int expVal)
        {
            float expLeft = 0;
            if (expVal > 100)
            {
                expLeft = expVal - 100;
                expVal = 100;
                  
            }
            fill.fillAmount = (expVal) / 100f;
        }
    }
}
