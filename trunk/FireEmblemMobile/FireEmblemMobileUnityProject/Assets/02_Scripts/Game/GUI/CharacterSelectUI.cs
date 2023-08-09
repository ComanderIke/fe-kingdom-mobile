using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace LostGrace
{
    public class CharacterSelectUI : UIMenu
    {
       
        [SerializeField] private Canvas sortingCanvas;
        [SerializeField] private Canvas charViewCanvas;
        [SerializeField] private CanvasGroup speechBubbleCanvasGroup;
        [SerializeField] private TextMeshProUGUI speechBubbleText;
        [SerializeField] private CanvasGroup charCirclesCanvasGroup;
      
        [SerializeField] private CanvasGroup charViewCanvasGroup;
        [SerializeField] private CanvasGroup charButtonsCanvasGroup;
        [SerializeField] private CanvasGroup partySizeCanvasGroup;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup newGameButtonCanvasGroup;
        [SerializeField] private CanvasGroup backButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private CharacterSelector characterSelector;
        [SerializeField] private UIPartyCharacterCircleController characterCircles;
        [SerializeField] private UIDetailedCharacterViewController characterViewController;
        
        public override void Show()
        {
            Debug.Log("Creating new party");
            Player.Instance.Party = new Party();
            Player.Instance.Party.onMemberRemoved += PartyChanged;
            Player.Instance.Party.onMemberAdded += PartyChanged;
            UpdateButtonState();
            StartCoroutine(ShowCoroutine());
            characterViewController.ShowBoonBaneSelection();
          
          
        }
        public void StartClicked()
        {
            MenuActionController.StartGame();
        }
        private void Update()
        {
            int remaining = Player.Instance.startPartyMemberCount - Player.Instance.Party.members.Count;
            if(remaining>0)
                speechBubbleText.text = "Select " + remaining + " more...";
            else
                speechBubbleText.text = "You are ready to go!";
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
            partySizeCanvasGroup.alpha = 0;
            charViewCanvas.enabled = true;
            yield return new WaitForSeconds(.5f);

            LeanTween.alphaCanvas(newGameButtonCanvasGroup, UIUtility.INACTIVE_CANVAS_GROUP_ALPHA,
                TweenUtility.fadeInDuration).setEase(TweenUtility.easeFadeIn);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(backButtonCanvasGroup);
            TweenUtility.FadeIn(charCirclesCanvasGroup);
            TweenUtility.FadeIn(charViewCanvasGroup);
            TweenUtility.FadeIn(charButtonsCanvasGroup);
            TweenUtility.FadeIn(partySizeCanvasGroup);
            characterSelector.Show(GameConfig.Instance.config.GetUnits());
            
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
            TweenUtility.FadeOut(partySizeCanvasGroup);
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
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (Player.Instance.Party.members.Count<Player.Instance.startPartyMemberCount)
            {
                newGameButtonCanvasGroup.alpha = UIUtility.INACTIVE_CANVAS_GROUP_ALPHA;
                newGameButtonCanvasGroup.GetComponent<Button>().interactable = false;
            }
            else
            {
                newGameButtonCanvasGroup.alpha = 1;
                newGameButtonCanvasGroup.GetComponent<Button>().interactable = true;
            }
        }
        public override void Hide()
        {
            Player.Instance.Party.onMemberAdded -= PartyChanged;
            Player.Instance.Party.onMemberRemoved -= PartyChanged;
            StartCoroutine(HideCoroutine());
        }
    }
}
