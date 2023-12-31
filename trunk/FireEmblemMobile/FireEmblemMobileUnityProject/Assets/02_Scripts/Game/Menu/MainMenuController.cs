using System;
using System.Collections;
using Audio;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using LostGrace;
using Menu;
using UnityEngine;

namespace Game.GUI
{
    public class MainMenuController : UIMenu
    {
        public static MainMenuController Instance;
        
        [SerializeField] private UIMenu optionsMenu;
        [SerializeField] private POIManager hubMenu;
        [SerializeField] private UIMenu selectFileUI;
        private static readonly int Show1 = Animator.StringToHash("Show");
        private static readonly int Side = Animator.StringToHash("Side");
       // [SerializeField] private Animator animator;
       [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup playButtonCanvasGroup;
        [SerializeField] private CanvasGroup optionsButtonCanvasGroup;
        [SerializeField] private CanvasGroup exitButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
        private static readonly int Selected = Animator.StringToHash("Selected");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Show();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        public override void Show()
        {
            // animator.SetBool(Show1, true);
            // animator.SetBool(Side, false);
                AudioSystem.Instance.ChangeAllMusic("MainMenuTheme");
            TweenUtility.FadeIn(canvasGroup);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(playButtonCanvasGroup);
            TweenUtility.FadeIn(optionsButtonCanvasGroup);
            TweenUtility.FadeIn(exitButtonCanvasGroup);
            //optionsButtonAnimator.SetBool(Selected, false);
           // playButtonAnimator.SetBool(Selected, false);
            base.Show();
        }
        
        public override void Hide()
        {
            // animator.SetBool(Show1, false);
            // animator.SetBool(Side, false);
            TweenUtility.FadeOut(canvasGroup, 0.7f);
            //base.Hide();
        }

        public void StartClicked()
        {
            HideOtherMenus(selectFileUI);
            selectFileUI.Show();
            Hide();
            // StartCoroutine(TransitionAnimation());

        }

        
        public void StartHub()
        {
            HideOtherMenus(hubMenu);
            StartCoroutine(TransitionAnimation());
        }

        IEnumerator TransitionAnimation()
        {
            TweenUtility.FadeOut(optionsButtonCanvasGroup);
            TweenUtility.FadeOut(exitButtonCanvasGroup);
            yield return new WaitForSeconds(.4f);
            TweenUtility.FadeOut(playButtonCanvasGroup);
            TweenUtility.FadeOut(titleCanvasGroup);
            GameSceneController.Instance.LoadSanctuary();
        }

        void HideOtherMenus(UIMenu menu)
        {
            if(optionsMenu!=menu)
                optionsMenu.Hide();
            //if(hubMenu!=menu)
            //    hubMenu.Hide();
            if(selectFileUI!=menu)
                selectFileUI.Hide();
        }
        public void OptionsClicked()
        {
            HideOtherMenus(optionsMenu);
          
            optionsMenu.Show();
            //optionsButtonAnimator.SetBool(Selected, true);
            Hide();
        }
        public void ExitClicked()
        {
            Application.Quit();
        }

    }
}
