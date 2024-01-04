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
       
       // [SerializeField] private Canvas sortingCanvas;
     //   [SerializeField] private Canvas charViewCanvas;
        [SerializeField] private CanvasGroup speechBubbleCanvasGroup;
        [SerializeField] private TextMeshProUGUI speechBubbleText;
        [SerializeField] private CanvasGroup charCirclesCanvasGroup;
      
        [SerializeField] private CanvasGroup charViewCanvasGroup;
        [SerializeField] private CanvasGroup charButtonsCanvasGroup;
        [SerializeField] private CanvasGroup partySizeCanvasGroup;
     

        [SerializeField] private Button newGameButton;
       // [SerializeField] private CanvasGroup newGameButtonCanvasGroup;
       
        [SerializeField] private CanvasGroup Fade;
        [SerializeField] private GoddessUI goddessUI;
        [SerializeField] private CharacterSelector characterSelector;
        [SerializeField] private UIPartyCharacterCircleController characterCircles;
        [SerializeField] private UIDetailedCharacterViewController characterViewController;
       
        
        public override void Show()
        {
            MyDebug.LogLogic("Creating new party");
            //disableBackground.gameObject.SetActive(false);
            Player.Instance.Party = new Party();
            Player.Instance.Party.onMemberRemoved += PartyChanged;
            Player.Instance.Party.onMemberAdded += PartyChanged;
            Player.Instance.Party.onActiveUnitChanged -= ActiveUnitChanged;
            Player.Instance.Party.onActiveUnitChanged += ActiveUnitChanged;
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
            //sortingCanvas.enabled = true;
           // newGameButtonCanvasGroup.alpha = 0;
          
   
            charCirclesCanvasGroup.alpha = 0;
            charViewCanvasGroup.alpha = 0;
            charButtonsCanvasGroup.alpha = 0;
            speechBubbleCanvasGroup.alpha = 0;
            partySizeCanvasGroup.alpha = 0;
            //charViewCanvas.enabled = true;
          //  yield return new WaitForSeconds(.5f);

            // LeanTween.alphaCanvas(newGameButtonCanvasGroup, UIUtility.INACTIVE_CANVAS_GROUP_ALPHA,
            //     TweenUtility.fadeInDuration).setEase(TweenUtility.easeFadeIn);
         
       
            TweenUtility.FadeIn(charCirclesCanvasGroup);
            TweenUtility.FadeIn(charViewCanvasGroup);
            TweenUtility.FadeIn(charButtonsCanvasGroup);
            TweenUtility.FadeIn(partySizeCanvasGroup);
            var units = GameConfig.Instance.GetPlayerUnits();
            
            characterSelector.Show(units);
            characterSelector.OnUnitSelected-=UnitSelected;
            characterSelector.OnUnitSelected+=UnitSelected;
            
            characterCircles.Show(Player.Instance.Party);
            yield return new WaitForSeconds(.2f);
            goddessUI.Show();
            yield return new WaitForSeconds(4.5f);
            TweenUtility.FadeIn(speechBubbleCanvasGroup);
            if (GameConfig.Instance.ConfigProfile.tutorialEnabled)
                yield return TutorialCoroutine();

        }

        void UnitSelected(Unit unit)
        {
            characterViewController.Show(unit);
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
            // TweenUtility.FadeOut(newGameButtonCanvasGroup);
            TweenUtility.FadeOut(speechBubbleCanvasGroup);
            goddessUI.Hide();
            //yield return new WaitForSeconds(0.5f);
            characterSelector.OnUnitSelected-=UnitSelected;
            TweenUtility.FadeOut(partySizeCanvasGroup);
            TweenUtility.FadeOut(charCirclesCanvasGroup);
            TweenUtility.FadeOut(charViewCanvasGroup);
            TweenUtility.FadeOut(charButtonsCanvasGroup);
            base.Hide();
           // yield return new WaitForSeconds(2.0f);
           
           //   sortingCanvas.enabled = false;
           //parent?.Show();
           //TweenUtility.FadeOut(Fade);
            // TweenUtility.FadeIn(Fade).setOnComplete(()=>
            // {
            //    
            // });
            yield return null;
        }

        void ActiveUnitChanged()
        {
            characterCircles.Show(Player.Instance.Party);
            characterSelector.UpdateUI();
        }
        void PartyChanged(Unit u)
        {
            characterCircles.Show(Player.Instance.Party);
            characterSelector.UpdateUI();
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (Player.Instance.Party.members.Count<Player.Instance.startPartyMemberCount)
            {
               // newGameButtonCanvasGroup.alpha = UIUtility.INACTIVE_CANVAS_GROUP_ALPHA;
               newGameButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Start Journey");
               newGameButton.interactable = false;
            }
            else
            {
               // newGameButtonCanvasGroup.alpha = 1;
               newGameButton.GetComponentInChildren<TextMeshProUGUI>().SetText("<bounce>Start Journey");
                newGameButton.interactable = true;
            }
        }
        public override void Hide()
        {
            Player.Instance.Party.onMemberAdded -= PartyChanged;
            Player.Instance.Party.onMemberRemoved -= PartyChanged;
            Player.Instance.Party.onActiveUnitChanged -= ActiveUnitChanged;
            StartCoroutine(HideCoroutine());
        }
    }
}
