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

    public Canvas canvas;
    private float width;
    private float height;
    private void Awake()
    {
        
    }

    private void Start()
    {
        battleCanvasGroup = battleCanvas.GetComponent<CanvasGroup>();
        RectTransform rawImageRect = image.GetComponent<RectTransform>();
       
        width =canvas.pixelRect.width/canvas.scaleFactor;
        height = canvas.pixelRect.height/canvas.scaleFactor;
        Debug.Log("width: "+width+" height: "+height);
        rawImageRect.sizeDelta = new Vector2(width, height);
        RenderTexture.width =(int) width;
        RenderTexture.height =(int)height;
        
    }
  

    public void Show()
    {
      
        GetComponent<Canvas>().enabled = true;
        RectTransform rectT = mask.rectTransform;
        rectT.sizeDelta = new Vector2(0, 0);
       
        LeanTween.size(rectT, new Vector2(width+maskMaxSize.x, height+maskMaxSize.y), fadeInTime).setEaseOutQuad();
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
