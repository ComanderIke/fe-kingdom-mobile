﻿using UnityEngine;
using UnityEngine.Serialization;

namespace GameCamera
{
    public class ViewOnGridMixin : CameraMixin
    {
        private int currentZoom = -1;

        [Range(0, 3)] public int zoom = 1;
        private float startDistance;

        private void Update()
        {
            if (Input.touchCount == 2)
            {
                // Vector2 touch0, touch1;
                // float distance;
                // touch0 = Input.GetTouch(0).position;
                // touch1 = Input.GetTouch(1).position;
                // distance = Vector2.Distance(touch0, touch1);
                //
                // if (startDistance == 0)
                //     startDistance = distance;
                // else if (distance - startDistance >= 100)
                // {
                //     zoom=0;
                //     startDistance = distance;
                // }
                // else if (distance - startDistance <= -100)
                // {
                //     zoom=1;
                //     startDistance = distance;
                // }
                // Debug.Log("Distance: " + distance+" Start: "+startDistance+ " Zoom: "+zoom);
            }
            else
            {
                startDistance = 0;
            }
            if (currentZoom != zoom)
            {
                currentZoom = zoom;
                var cam = CameraSystem.camera;
                switch (zoom)
                {
                    case 0:
                        cam.orthographicSize = 3f;

                        cam.transform.localPosition = new Vector3(5.33f, 3f, cam.transform.localPosition.z);
                        break;
                    case 1:
                        cam.orthographicSize = 4f;
  
                        cam.transform.localPosition = new Vector3(7, 4f, cam.transform.localPosition.z);
                        break;
                    // case 2:
                    //     cam.orthographicSize = 5f;
                    //
                    //     cam.transform.localPosition = new Vector3(7, 4f, cam.transform.localPosition.z);
                    //     break;
                    // case 3:
                    //     cam.orthographicSize = 6f;
                    //     cam.transform.localPosition = new Vector3(9, 5f, cam.transform.localPosition.z);
                    //     break;
                }
            }
        }
    }
}