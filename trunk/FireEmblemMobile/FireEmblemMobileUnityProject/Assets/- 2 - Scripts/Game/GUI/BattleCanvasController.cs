using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvasController : MonoBehaviour
{
    public RawImage image;
    public GameObject battleCanvas;
    private CanvasGroup battleCanvasGroup;
    private void OnEnable()
    {
        battleCanvasGroup = battleCanvas.GetComponent<CanvasGroup>();
    }

    public void Show()
    {
      
        GetComponent<Canvas>().enabled = true;
        RectTransform rectT = image.rectTransform;
        rectT.sizeDelta = new Vector2(0, 0);
     
        LeanTween.size(rectT, new Vector2(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y), 1.2f).setEaseOutQuad();
        //LeanTween.scaleY(battleCanvas,1,1.2f).setEaseOutQuad();
        LeanTween.alphaCanvas(battleCanvasGroup,.95f, 1.2f).setEaseOutQuad();
    }

    public void Hide()
    {
        RectTransform rectT = image.rectTransform;
        LeanTween.size(rectT, new Vector2(0, 0), 0.4f).setEaseInQuad().setOnComplete(()=>GetComponent<Canvas>().enabled = false);
       // LeanTween.scaleY(battleCanvas,0,0.4f).setEaseInQuad();
        LeanTween.alphaCanvas(battleCanvasGroup,0, .25f).setEaseInQuad();
       
    }
}
