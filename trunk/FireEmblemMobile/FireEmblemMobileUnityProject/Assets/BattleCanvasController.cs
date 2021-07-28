using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCanvasController : MonoBehaviour
{
    public RawImage image;

    private void OnEnable()
    {
        RectTransform rectT = image.rectTransform;
        rectT.sizeDelta = new Vector2(0, 0);
        LeanTween.size(rectT, new Vector2(1920, 1080), 1.2f).setEaseOutQuad();
    }
}
