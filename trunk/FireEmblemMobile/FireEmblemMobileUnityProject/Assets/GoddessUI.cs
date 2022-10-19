using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class GoddessUI : MonoBehaviour
    {
        [SerializeField] private RectTransform rectT;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private ParticleSystem glow;
        [SerializeField] private ParticleSystem clouds;
        [SerializeField] private Animator animator;
        private void OnEnable()
        {
            Show();
        }

        private bool show = true;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                show = !show;
                if(show)
                    Show();
                else
                {
                    Hide();
                }
            }
           
        }

     

        IEnumerator ShowGoddessAnimation()
        {
            
            LeanTween.cancel(canvasGroup.gameObject);
            StopCoroutine(nameof(ShowGoddessAnimation));
            StopCoroutine(nameof(HideGoddessAnimation));
            animator.enabled = false;
            canvasGroup.alpha = 0;
           
            clouds.gameObject.SetActive(true);
            clouds.Play();
            yield return new WaitForSeconds(.6f);
            particles.gameObject.SetActive(true);
            particles.Play();
            yield return new WaitForSeconds(1.8f);
            Debug.Log("Before");
            int tweenId = TweenUtility.SlowFadeIn(canvasGroup).id;
            rectT.anchoredPosition = new Vector3(rectT.anchoredPosition.x, -70);
            LeanTween.moveY(rectT, 0, .5f).setEaseOutQuad().setDelay(.1f);
            yield return new WaitWhile(()=>LeanTween.isTweening(tweenId));
            Debug.Log("After");
            glow.gameObject.SetActive(true);
            glow.Play();
            animator.enabled = true;

        }

        IEnumerator HideGoddessAnimation()
        {
            animator.enabled = false;
            LeanTween.cancel(canvasGroup.gameObject);
            StopCoroutine(nameof(ShowGoddessAnimation));
            StopCoroutine(nameof(HideGoddessAnimation));
           
            glow.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            LeanTween.moveY(rectT, -70, .5f).setEaseInQuad().setDelay(1.45f);
            yield return TweenUtility.FadeOut(canvasGroup).setDelay(1.5f);
            yield return new WaitForSeconds(0.5f);
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            clouds.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public void Show()
        {
            StartCoroutine(ShowGoddessAnimation());
        }
        public void Hide()
        {
            StartCoroutine(HideGoddessAnimation());
           
        }
    }
}
