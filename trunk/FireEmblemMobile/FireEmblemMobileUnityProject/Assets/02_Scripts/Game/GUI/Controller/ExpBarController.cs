using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GUI.Controller
{
    public class ExpBarController : MonoBehaviour
    {
        
        [SerializeField] Image fill;
        [SerializeField] private UIFillStrechedImage fillAlternate;
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
        private bool animate;
        [SerializeField] private float tmpFillSecondsPerUnit= 0.1f;
        [SerializeField] private float fillSecondsPerUnit= 0.5f;
        [SerializeField] private ParticleSystem fillParticles;
        [SerializeField] private Image faceSprite;
        [SerializeField] private Image preview;

        public Action onFinished;
        public void ParticleArrived()
        {
            addedExp--;
            currentExp++;
            if (currentExp > 100)
            {
                currentExp -= 100;
               
            }

            if(fill!=null)
                fill.fillAmount = currentExp / 100f;
            if(fillAlternate!=null)
                fillAlternate.SetFill(currentExp / 100f);
            countingText?.SetText(currentExp.ToString());
            if (addedExp <=0)
            {
                Debug.Log("AllParticlesArrived!");
                animate = false;
                onFinished?.Invoke();

            }
        }
        

        IEnumerator AnimatedText()
        {
            yield return new WaitForSeconds(.3f);
            if(fillParticles!=null)
                fillParticles.Play();
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
                   
                    if(fullFeedbacks!=null)
                        fullFeedbacks.PlayFeedbacks();
                    currentExp = 0;

                }

                if(fill!=null)
                    fill.fillAmount = currentExp / 100f;
                if(fillAlternate!=null)
                    fillAlternate.SetFill(currentExp / 100f);
                
                countingText?.SetText(currentExp.ToString());
                yield return new WaitForSeconds(fillSecondsPerUnit);
            }
            if(fillParticles!=null)
                fillParticles.Stop();
            yield return new WaitForSeconds(0.8f);
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
            // Debug.Log("Show With Animatedtext: CurrentExp: "+currentExp+" Gained: "+addedExp);
            
            this.addedExp += expgained;
            this.tmpAddedExp += expgained;
            StartCoroutine(AnimatedText());
            StartCoroutine(AnimatedTmpFillText());
        }

        public void UpdatePreview(int expVal)
        {
            // Debug.Log("Update Preview");
            preview.fillAmount = expVal / 100f;
        }
        public void UpdateInstant(int expVal)
        {
            // Debug.Log("UpdateInstant: "+expVal);
            if (animate)
            {
                return;
            }
            if (expVal > 100)
            {
                expVal -= 100;
                  
            }

            currentExp = expVal;
            tmpExp = currentExp;


            if (fill != null)
            {
                fill.fillAmount = currentExp / 100f;
                if (Math.Abs(fill.fillAmount - 1) < 0.1f)
                    fill.fillAmount = 0;
            }

            if (fillAlternate != null)
            {
                fillAlternate.SetFill(currentExp / 100f);
                if (Math.Abs(currentExp/100f - 1) < 0.1f)
                    fillAlternate.SetFill(0);
            }
               
          
           
            tmpFill.fillAmount =currentExp / 100f;
            countingText?.SetText(currentExp.ToString());
            addedExp = 0;
            tmpAddedExp = 0;
        }

        public void Show(Sprite sprite, int currentExp)
        {
            // Debug.Log("Show Normal");
            if(hideFeedbacks!=null)
                hideFeedbacks.StopFeedbacks();
            if(showFeedbacks!=null)
                showFeedbacks.PlayFeedbacks();
            if(faceSprite!=null)
                faceSprite.sprite = sprite;
            UpdateInstant(currentExp);
            //TweenUtility.FadeIn(canvasGroup);
        }

        
        public void Hide()
        {
            if(showFeedbacks!=null)
                showFeedbacks.StopFeedbacks();
            if (hideFeedbacks != null)
            {
                hideFeedbacks.PlayFeedbacks();
            }

            //TweenUtility.FadeOut(canvasGroup);
        }

      
    }
}