using System;
using Game.GameActors.Units;
using Game.States;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class ExpBarController : MonoBehaviour
    {
        [SerializeField] Image fill;
        [SerializeField] int currentExp;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] UIAnimatedCountingText countingText;
        private int addedExp;
        private bool animation;


        public Action onAllParticlesArrived;
        public void ParticleArrived()
        {
            addedExp--;
            currentExp++;
            if (currentExp > 100)
                currentExp -= 100;
            fill.fillAmount = currentExp / 100f;
            countingText?.SetText(currentExp.ToString());
            if (addedExp <=0)
            {
                Debug.Log("AllParticlesArrived!");
                animation = false;
                onAllParticlesArrived?.Invoke();

            }
        }
        
        public void UpdateAnimated(int addedExp)
        {
            Debug.Log("CurrentExp: "+currentExp+" Gained: "+addedExp);
            animation = true;
            this.addedExp +=  addedExp;
            //countingText?.SetTextCounting(currentExp, currentExp+this.addedExp);
        }



        public void UpdateInstant(int expVal)
        {
            if (animation)
            {
                return;
            }
            if (expVal > 100)
            {
                expVal -= 100;
                  
            }

            currentExp = expVal;
            fill.fillAmount = currentExp / 100f;
            if (Math.Abs(fill.fillAmount - 1) < 0.1f)
                fill.fillAmount = 0;
            countingText?.SetText(currentExp.ToString());
        }

        public void Show(int CurrentExp)
        {
            UpdateInstant(currentExp);
            TweenUtility.FadeIn(canvasGroup);
        }
        public void Hide()
        {
            TweenUtility.FadeOut(canvasGroup);
        }
    }
}