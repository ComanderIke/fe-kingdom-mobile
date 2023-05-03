using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInScript : MonoBehaviour
{
 
    public Camera cam;
    private float orthoSizeStart;
    public Vector3 startPos;
    public float OrthoSizeZoomIn = 2.5f;
  
    // Start is called before the first frame update
    void Start()
    {
        orthoSizeStart = cam.orthographicSize;
        startPos = cam.transform.position;
    }

    // Update is called once per frame
    

    public void ZoomIn(Vector3 pos)
    {
        LeanTween.move(cam.gameObject, new Vector3(pos.x, pos.y, cam.transform.position.z), 1.0f).setEaseInOutQuad();
        LeanTween.value(gameObject, orthoSizeStart, OrthoSizeZoomIn, 1.0f).setEaseInOutQuad().setOnUpdate(val =>
        {
            cam.orthographicSize = val;
        });
    }
    public void ZoomOut()
    {
        LeanTween.move(cam.gameObject, startPos, 1.0f).setEaseInOutQuad();
        LeanTween.value(gameObject, OrthoSizeZoomIn, orthoSizeStart, 1.0f).setEaseInOutQuad().setOnUpdate(val =>
        {
            cam.orthographicSize = val;
        });
    }
}
