﻿using System;
using System.Collections;
using Game.GameActors.Units;
using Game.States;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Game.GUI
{
    public class ExpBarController : MonoBehaviour
    {
        [SerializeField] Image fill;
        [SerializeField] Image tmpFill;
        [SerializeField] int currentExp;
        [SerializeField] int tmpExp;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] UIAnimatedCountingText countingText;
        [SerializeField] private MMF_Player showFeedbacks;
        [SerializeField] private MMF_Player fullFeedbacks;
        [SerializeField] private MMF_Player hideFeedbacks;
        private int addedExp;
        private int tmpAddedExp;
        private bool animation;
        [SerializeField] private float tmpFillSecondsPerUnit= 0.1f;
        [SerializeField] private float fillSecondsPerUnit= 0.5f;


        public Action onFinished;
        public void ParticleArrived()
        {
            addedExp--;
            currentExp++;
            if (currentExp > 100)
            {
                currentExp -= 100;
               
            }

            fill.fillAmount = currentExp / 100f;
            countingText?.SetText(currentExp.ToString());
            if (addedExp <=0)
            {
                Debug.Log("AllParticlesArrived!");
                animation = false;
                onFinished?.Invoke();

            }
        }
        
        public void UpdateWithAnimatedParticles(int addedExp)
        {
            Debug.Log("CurrentExp: "+currentExp+" Gained: "+addedExp);
            animation = true;
            this.addedExp +=  addedExp;
            //countingText?.SetTextCounting(currentExp, currentExp+this.addedExp);
        }

        IEnumerator AnimatedText()
        {
            while (addedExp != 0)
            {
                addedExp--;
                currentExp++;
                if (currentExp > 100)
                {
                    currentExp -= 100;
                    
                }
                else if (currentExp == 100)
                {
                    fullFeedbacks.PlayFeedbacks();
                }

                fill.fillAmount = currentExp / 100f;
                countingText?.SetText(currentExp.ToString());
                yield return new WaitForSeconds(fillSecondsPerUnit);
            }

            yield return new WaitForSeconds(1.1f);
            onFinished?.Invoke();
            
        }
        IEnumerator AnimatedTmpFillText()
        {
            while (tmpAddedExp != 0)
            {
                tmpAddedExp--;
                tmpExp++;
                if (tmpExp > 100)
                    tmpExp -= 100;
                tmpFill.fillAmount = tmpExp / 100f;
                yield return new WaitForSeconds(tmpFillSecondsPerUnit);
            }
            
        }
        public void UpdateWithAnimatedTextOnly(int expgained)
        {
            //Debug.Log("CurrentExp: "+currentExp+" Gained: "+addedExp);
            
            this.addedExp += expgained;
            this.tmpAddedExp += expgained;
            StartCoroutine(AnimatedText());
            StartCoroutine(AnimatedTmpFillText());
        }

        public void UpdateInstant(int expVal)
        {
           // Debug.Log("UpdateInstant: "+expVal);
            if (animation)
            {
                return;
            }
            if (expVal > 100)
            {
                expVal -= 100;
                  
            }

            currentExp = expVal;
            tmpExp = currentExp;
            //Debug.Log("Currentexp: "+currentExp);
            fill.fillAmount = currentExp / 100f;
          
            if (Math.Abs(fill.fillAmount - 1) < 0.1f)
                fill.fillAmount = 0;
            tmpFill.fillAmount = fill.fillAmount;
            countingText?.SetText(currentExp.ToString());
        }

        public void Show(int CurrentExp)
        {
            showFeedbacks.PlayFeedbacks();
            UpdateInstant(currentExp);
            //TweenUtility.FadeIn(canvasGroup);
        }
        public void Hide()
        {
            showFeedbacks.StopFeedbacks();
            hideFeedbacks.PlayFeedbacks();
            //TweenUtility.FadeOut(canvasGroup);
        }

      
    }
}