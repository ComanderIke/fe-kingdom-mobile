using System.Collections;
using UnityEngine;

namespace Game.GUI
{
    public class MainMenuController : UIMenu
    {
        public static MainMenuController Instance;
        
        [SerializeField] private UIMenu optionsMenu;
        [SerializeField] private UIMenu hubMenu;
        private static readonly int Show1 = Animator.StringToHash("Show");
        private static readonly int Side = Animator.StringToHash("Side");
        [SerializeField] private Animator animator;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private CanvasGroup playButtonCanvasGroup;
        [SerializeField] private CanvasGroup optionsButtonCanvasGroup;
        [SerializeField] private CanvasGroup exitButtonCanvasGroup;
        [SerializeField] private CanvasGroup Fade;
      
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
            animator.SetBool(Show1, true);
            animator.SetBool(Side, false);
            TweenUtility.FadeIn(titleCanvasGroup);
            TweenUtility.FadeIn(playButtonCanvasGroup);
            TweenUtility.FadeIn(optionsButtonCanvasGroup);
            TweenUtility.FadeIn(exitButtonCanvasGroup);
            base.Show();
        }
        
        public override void Hide()
        {
            animator.SetBool(Show1, false);
            animator.SetBool(Side, false);
            base.Hide();
        }

        public void StartClicked()
        {
            StartCoroutine(TransitionAnimation());

        }
        IEnumerator TransitionAnimation()
        {
            TweenUtility.FadeOut(optionsButtonCanvasGroup);
            TweenUtility.FadeOut(exitButtonCanvasGroup);
            yield return new WaitForSeconds(.4f);
            TweenUtility.FadeIn(Fade).setOnComplete(() =>
            {
                hubMenu.Show();
                TweenUtility.FadeOut(Fade);
            });
            TweenUtility.FadeOut(playButtonCanvasGroup);
            TweenUtility.FadeOut(titleCanvasGroup);

        }

        public void OptionsClicked()
        {
            optionsMenu.Show();
        }
        public void ExitClicked()
        {
            Application.Quit();
        }

    }
}
