using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace LostGrace
{
    public class CharacterSelectUI : UIMenu
    {
        [SerializeField] private Canvas sortingCanvas;
        [SerializeField] private Canvas charViewCanvas;
        [SerializeField] private CanvasGroup speechBubbleCanvasGroup;
        [SerializeField] private CanvasGroup charCirclesCanvasGroup;
      
        [SerializeField] private CanvasGroup charViewCanvasGroup;
        [SerializeField] private CanvasGroup charButtonsCanvasGroup;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup newGameButtonCanvasGroup;
        [SerializeField] private CanvasGroup backButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private CharacterSelector characterSelector;
        [SerializeField] private UIPartyCharacterCircleController characterCircles;
       
        
        public override void Show()
        {
            StartCoroutine(ShowCoroutine());
            Player.Instance.Party = ScriptableObject.CreateInstance<Party>();
            Player.Instance.Party.onMemberRemoved += PartyChanged;
            Player.Instance.Party.onMemberAdded += PartyChanged;
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
            charViewCanvas.enabled = true;
            yield return new WaitForSeconds(.5f);
           
            TweenUtility.FadeIn(newGameButtonCanvasGroup);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(backButtonCanvasGroup);
            TweenUtility.FadeIn(charCirclesCanvasGroup);
            TweenUtility.FadeIn(charViewCanvasGroup);
            TweenUtility.FadeIn(charButtonsCanvasGroup);
            characterSelector.Show(GameConfig.Instance.config.selectableCharacters);
            
            characterCircles.Show(Player.Instance.Party);
            yield return new WaitForSeconds(.6f);
            goddessUI.Show();
            yield return new WaitForSeconds(4.5f);
            TweenUtility.FadeIn(speechBubbleCanvasGroup);
            if (GameConfig.Instance.config.tutorial)
                yield return TutorialCoroutine();

        }

        IEnumerator TutorialCoroutine()
        {
            yield return null;
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

        void PartyChanged(Unit u)
        {
            characterCircles.Show(Player.Instance.Party);
        }
        public override void Hide()
        {
            Player.Instance.Party.onMemberAdded -= PartyChanged;
            Player.Instance.Party.onMemberRemoved -= PartyChanged;
            StartCoroutine(HideCoroutine());
        }
    }
}
