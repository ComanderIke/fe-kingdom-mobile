using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvasController : MonoBehaviour
{
    public RawImage image;
    public Image mask;
    public GameObject battleCanvas;
    private CanvasGroup battleCanvasGroup;
    public Vector2 maskMaxSize;
    public float fadeInTime = 1.2f;
    public float fadeOutTime = 0.25f;
    public RenderTexture RenderTexture;

    private void Awake()
    {
        battleCanvasGroup = battleCanvas.GetComponent<CanvasGroup>();
        RectTransform rawImageRect = image.GetComponent<RectTransform>();
        rawImageRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        RenderTexture.width = Screen.width;
        RenderTexture.height = Screen.height;
    }

  

    public void Show()
    {
      
        GetComponent<Canvas>().enabled = true;
        RectTransform rectT = mask.rectTransform;
        rectT.sizeDelta = new Vector2(0, 0);
       
        LeanTween.size(rectT, new Vector2(Screen.width+maskMaxSize.x, Screen.height+maskMaxSize.y), fadeInTime).setEaseOutQuad();
        battleCanvasGroup.alpha = 1;
        //LeanTween.scaleY(battleCanvas,1,1.2f).setEaseOutQuad();
        //LeanTween.alphaCanvas(battleCanvasGroup,.95f, fadeInTime).setEaseOutQuad();
    }

    public void Hide()
    {
        RectTransform rectT = mask.rectTransform;
        LeanTween.size(rectT, new Vector2(0, 0), fadeOutTime).setEaseInQuad().setOnComplete(()=>GetComponent<Canvas>().enabled = false);
       // LeanTween.scaleY(battleCanvas,0,0.4f).setEaseInQuad();
       // LeanTween.alphaCanvas(battleCanvasGroup,0, fadeOutTime).setEaseInQuad();
       
    }
}
