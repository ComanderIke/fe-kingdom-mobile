﻿using UnityEngine;

namespace Assets.GameCamera
{
    public class ViewOnGridMixin : CameraMixin
    {
        private new Camera camera;
        private int currentZoom = -1;
        private Camera uiCamera;

        [Range(0, 3)] public int Zoom = 0;

        private void Start()
        {
            camera = Camera.main;
            uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        }

        private void Update()
        {
            if (currentZoom != Zoom)
            {
                currentZoom = Zoom;
                switch (Zoom)
                {
                    case 0:
                        camera.orthographicSize = 5.32f;
                        uiCamera.orthographicSize = 5.32f;
                        camera.transform.localPosition = new Vector3(3, 4.38f, camera.transform.localPosition.z);
                        break;
                    case 1:
                        camera.orthographicSize = 7.1f;
                        uiCamera.orthographicSize = 7.1f;
                        camera.transform.localPosition = new Vector3(4, 5.85f, camera.transform.localPosition.z);
                        break;
                    case 2:
                        camera.orthographicSize = 8.9f;
                        uiCamera.orthographicSize = 8.9f;
                        camera.transform.localPosition = new Vector3(5, 7.37f, camera.transform.localPosition.z);
                        break;
                    case 3:
                        camera.orthographicSize = 10.72f;
                        uiCamera.orthographicSize = 10.72f;
                        camera.transform.localPosition = new Vector3(6, 8.87f, camera.transform.localPosition.z);
                        break;
                }
            }
        }
    }
}