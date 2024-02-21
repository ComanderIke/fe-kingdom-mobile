using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class UIAreaCompletedController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInTime=1.2f;
        [SerializeField] private float stayDuration=2f;
        [SerializeField] private float fadeOutTime=1f;
        // Start is called before the first frame update
        void Start()
        {
            AreaGameManager.OnAreaCompleted -= Show;
            AreaGameManager.OnAreaCompleted += Show;
        }

        private void OnDestroy()
        {
            AreaGameManager.OnAreaCompleted -= Show;
        }

        // Update is called once per frame
        void Show()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setEaseInOutQuad().setOnComplete(() =>
                LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setEaseInOutQuad().setDelay(stayDuration));
        }
    }
}
