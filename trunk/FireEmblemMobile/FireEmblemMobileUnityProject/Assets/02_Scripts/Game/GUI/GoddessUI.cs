using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LostGrace
{
    public class GoddessUI : MonoBehaviour
    {
        [SerializeField] private RectTransform rectT;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private ParticleSystem particles;
     //   [SerializeField] private ParticleSystem glow;
        [SerializeField] private ParticleSystem clouds;
        [SerializeField] private Animator animator;
        [SerializeField] private Image godSprite;

        [SerializeField] private God god;
        // private void OnEnable()
        // {
        //     Show();
        // }
        

     

        IEnumerator ShowGoddessAnimation()
        {
            godSprite.sprite = god.DialogSpriteSet.BodySprite;
            godSprite.material.SetTexture("_MainTex", god.DialogSpriteSet.BodySprite.texture);
            godSprite.material.SetColor("_Color", god.bodyOutlineColor);
            LeanTween.cancel(canvasGroup.gameObject);
            StopCoroutine(nameof(ShowGoddessAnimation));
            StopCoroutine(nameof(HideGoddessAnimation));
            animator.enabled = false;
            canvasGroup.alpha = 0;
       //     glow.gameObject.SetActive(false);
            clouds.gameObject.SetActive(true);
            clouds.Play();
            yield return new WaitForSeconds(.6f);
            particles.gameObject.SetActive(true);
            particles.Play();
            yield return new WaitForSeconds(1.8f);
         
            int tweenId = TweenUtility.SlowFadeIn(canvasGroup).id;
            rectT.anchoredPosition = new Vector3(rectT.anchoredPosition.x, -70);
            LeanTween.moveY(rectT, 0, .5f).setEaseOutQuad().setDelay(.1f);
            yield return new WaitWhile(()=>LeanTween.isTweening(tweenId));
         
          //  glow.gameObject.SetActive(true);
         //   glow.Play();
            animator.enabled = true;

        }

        IEnumerator HideGoddessAnimation()
        {
            animator.enabled = false;
            LeanTween.cancel(canvasGroup.gameObject);
            StopCoroutine(nameof(ShowGoddessAnimation));
            StopCoroutine(nameof(HideGoddessAnimation));
           
        //    glow.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            LeanTween.moveY(rectT, -70, .5f).setEaseInQuad().setDelay(1.45f);
            yield return TweenUtility.FadeOut(canvasGroup).setDelay(1.5f);
            yield return new WaitForSeconds(0.5f);
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            clouds.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(ShowGoddessAnimation());
        }
        public void Hide()
        {
            StopAllCoroutines();
            StartCoroutine(HideGoddessAnimation());
           
        }
    }
}
