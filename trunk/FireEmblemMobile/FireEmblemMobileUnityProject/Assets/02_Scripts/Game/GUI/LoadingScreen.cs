using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class LoadingScreen : MonoBehaviour
    {
        public CanvasGroup Fade;
        public GameObject Content;
        public Action onBlack;

        public void Show()
        {
            gameObject.SetActive(true);
            LeanTween.alphaCanvas(Fade, 1, .6f).setEaseInQuad().setOnComplete(()=>
            {
                onBlack?.Invoke();
                Content.SetActive(true);
                LeanTween.alphaCanvas(Fade, 0, .4f).setEaseOutQuad();
            });
            
        }
        public void Hide()
        {
            LeanTween.alphaCanvas(Fade, 1, .6f).setEaseInQuad().setOnComplete(()=>
            {
                
                Content.SetActive(false);
                LeanTween.alphaCanvas(Fade, 0, .4f).setEaseOutQuad().setOnComplete(()=>  gameObject.SetActive(false));
            });

        }
    }
}
