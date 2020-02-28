using Assets.Scripts.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ClampCameraMixin: CameraMixin
{
    public float uiHeight = 535;
    public float referenceHeight = 1920;
    private float boundsBorder = 1;
    private float maxX;
    private float minX;
    private float maxY;
    private float minY;
    private new Camera camera;
    private float vertExtent;
    private float horzExtent;
    private int gridWidth;
    private int gridHeight;

    private void Start()
    {
        camera = Camera.main;
        UpdateBounds();

    }
   
    private void LateUpdate()
    {
        if (camera.orthographicSize != vertExtent)
        {
            UpdateBounds();
        }
        
        if (this.transform.localPosition.x < minX ||
            this.transform.localPosition.x > maxX ||
            this.transform.localPosition.y < minY ||
            this.transform.localPosition.y > maxY)
        {
            this.transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x, minX, maxX), 
                Mathf.Clamp(transform.localPosition.y, minY, maxY), 
                this.transform.localPosition.z);
        }
    }
    private void UpdateBounds()
    {
        vertExtent = camera.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        float vertExtentWithoutGUI = vertExtent * ((Screen.height - (uiHeight * Screen.height / referenceHeight)) / Screen.height);//GUI takes some space1385 are the number of pixels for NonGUI
        minX = -boundsBorder;
        maxX = gridWidth - 2 * horzExtent + boundsBorder;
        minY = -boundsBorder;
        maxY = gridHeight - 2 * vertExtentWithoutGUI - 1 + 1+boundsBorder;
    }

    public ClampCameraMixin GridHeight(int gridHeight)
    {
        this.gridHeight = gridHeight;
        return this;
    }
    public ClampCameraMixin GridWidth(int gridWidth)
    {
        this.gridWidth = gridWidth;
        return this;
    }
    public ClampCameraMixin BoundsBorder(int boundsBorder)
    {
        this.boundsBorder = boundsBorder;
        return this;
    }

}

