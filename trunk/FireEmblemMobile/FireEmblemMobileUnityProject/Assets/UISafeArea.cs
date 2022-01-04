using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeArea : MonoBehaviour
{

    public Canvas canvas;
    private RectTransform panelSafeArea;
    private ScreenOrientation currentOrientation = ScreenOrientation.Landscape;

    private Rect currentSaveArea = new Rect();

    void Start()
    {
        panelSafeArea = GetComponent<RectTransform>();
        currentOrientation = Screen.orientation;
        currentSaveArea = Screen.safeArea;
        
        ApplySafeArea();
    }
    

    void ApplySafeArea()
    {
        if (panelSafeArea == null)
            return;


        Rect safeArea = Screen.safeArea;

        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = safeArea.position+safeArea.size;

        minAnchor.x /= canvas.pixelRect.width;
        minAnchor.y /= canvas.pixelRect.height;
        
        maxAnchor.x /= canvas.pixelRect.width;
        maxAnchor.y /= canvas.pixelRect.height;

        panelSafeArea.anchorMin = minAnchor;
        panelSafeArea.anchorMax = maxAnchor;

        currentOrientation = Screen.orientation;
        currentSaveArea = Screen.safeArea;
    }

    private void Update()
    {
        if (currentOrientation != Screen.orientation || currentSaveArea != Screen.safeArea)
        {
            ApplySafeArea();
        }
    }
}
