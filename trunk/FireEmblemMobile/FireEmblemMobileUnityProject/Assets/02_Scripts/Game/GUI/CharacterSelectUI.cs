using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class CharacterSelectUI : UIMenu
    {
        [SerializeField] private Canvas sortingCanvas;
        [SerializeField] private CanvasGroup speechBubbleCanvasGroup;
        [SerializeField] private CanvasGroup charCirclesCanvasGroup;
        [SerializeField] private CanvasGroup charViewCanvasGroup;
        [SerializeField] private CanvasGroup charButtonsCanvasGroup;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup newGameButtonCanvasGroup;
        [SerializeField] private CanvasGroup backButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        [SerializeField] private GoddessUI goddessUI;
       
        
        public override void Show()
        {
            StartCoroutine(ShowCoroutine());
        }

        IEnumerator ShowCoroutine()
        {
            base.Show();
            sortingCanvas.enabled = true;
            newGameButtonCanvasGroup.alpha = 0;
            titleCanvasGroup.alpha = 0;
            backButtonCanvasGroup.alpha = 0;
            charCirclesCanvasGroup.alpha = 0;
            charViewCanvasGroup.alpha = 0;
            charButtonsCanvasGroup.alpha = 0;
            speechBubbleCanvasGroup.alpha = 0;
            yield return new WaitForSeconds(.5f);
           
            TweenUtility.FadeIn(newGameButtonCanvasGroup);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(backButtonCanvasGroup);
            TweenUtility.FadeIn(charCirclesCanvasGroup);
            TweenUtility.FadeIn(charViewCanvasGroup);
            TweenUtility.FadeIn(charButtonsCanvasGroup);
            yield return new WaitForSeconds(.6f);
            goddessUI.Show();
            yield return new WaitForSeconds(4.5f);
            TweenUtility.FadeIn(speechBubbleCanvasGroup);

        }
        
        
        
        public override void BackClicked()
        {
            Hide();
            
        }
        IEnumerator HideCoroutine()
        {
            TweenUtility.FadeOut(backButtonCanvasGroup);
            TweenUtility.FadeOut(newGameButtonCanvasGroup);
            TweenUtility.FadeOut(speechBubbleCanvasGroup);
            goddessUI.Hide();
            yield return new WaitForSeconds(0.5f);
           
            TweenUtility.FadeOut(titleCanvasGroup);
            TweenUtility.FadeOut(charCirclesCanvasGroup);
            TweenUtility.FadeOut(charViewCanvasGroup);
            TweenUtility.FadeOut(charButtonsCanvasGroup);
            yield return new WaitForSeconds(2.0f);
            
            TweenUtility.FadeIn(Fade).setOnComplete(()=>
            {
                base.Hide();
                sortingCanvas.enabled = false;
                parent?.Show();
                TweenUtility.FadeOut(Fade);
            });
        }
        public override void Hide()
        {
            StartCoroutine(HideCoroutine());
        }
    }
}
