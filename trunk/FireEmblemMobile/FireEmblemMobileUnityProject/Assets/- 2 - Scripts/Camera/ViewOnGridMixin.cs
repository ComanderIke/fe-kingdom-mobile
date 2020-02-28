using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ViewOnGridMixin : CameraMixin
{
    [Range(0,3)]
    public int zoom = 0;
    private int currentZoom = -1;
    private new Camera camera;
    private new Camera uiCamera;

    private void Start()
    {
        camera = Camera.main;
        uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (currentZoom != zoom)
        {
            currentZoom = zoom;
            if (zoom == 0)
            {
                camera.orthographicSize = 5.32f;
                uiCamera.orthographicSize = 5.32f;
                camera.transform.localPosition = new Vector3(3, 4.38f, camera.transform.localPosition.z);

            }
            else if (zoom ==1)
            {
                camera.orthographicSize = 7.1f;
                uiCamera.orthographicSize = 7.1f;
                camera.transform.localPosition = new Vector3(4, 5.85f, camera.transform.localPosition.z);

            }
            else if (zoom == 2)
            {
                camera.orthographicSize = 8.9f;
                uiCamera.orthographicSize = 8.9f;
                camera.transform.localPosition = new Vector3(5, 7.37f, camera.transform.localPosition.z);

            }
            else if (zoom == 3)
            {
                camera.orthographicSize = 10.72f;
                uiCamera.orthographicSize = 10.72f;
                camera.transform.localPosition = new Vector3(6, 8.87f, camera.transform.localPosition.z);

            }
        }
    }
}
