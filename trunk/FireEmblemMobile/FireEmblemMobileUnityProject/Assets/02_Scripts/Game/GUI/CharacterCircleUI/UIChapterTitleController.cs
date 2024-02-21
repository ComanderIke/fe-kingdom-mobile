using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class UIChapterTitleController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float stayDuration;
        [SerializeField] private float fadeOutTime;
        void Start()
        {
            AreaGameManager.OnAreaStarted -= Show;
            AreaGameManager.OnAreaStarted += Show;
           

        }
        private void OnDestroy()
        {
            AreaGameManager.OnAreaStarted -= Show;
        }
        void Show()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            LeanTween.alphaCanvas(canvasGroup, 1, fadeInTime).setEaseInOutQuad().setOnComplete(() =>
                LeanTween.alphaCanvas(canvasGroup, 0, fadeOutTime).setEaseInOutQuad().setDelay(stayDuration)
                    .setOnComplete(() => canvasGroup.blocksRaycasts = false));
        }
        

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
