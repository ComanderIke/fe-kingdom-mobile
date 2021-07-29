using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoCamZoomController : MonoBehaviour
{
  
     public float zoomSpeed = 0.1f;
     public bool fakeZoomWithScale=false;
     public Transform fakeZoomParentObject;// all objects that need to be scaled, and this camera object, should be children of this transform.

     private void Update()
     {
         if (Input.GetKeyDown(KeyCode.U))
         {
             ZoomOrthoCamera(-Vector3.forward, true);
         }
     }

     private void ZoomOrthoCamera(Vector3 zoomToward, bool isZoomingIn)
     {
         float negSpeed = zoomSpeed * (isZoomingIn ? 1 : -1);
         Vector3 camToTarget = zoomToward - transform.position;
         if (!fakeZoomParentObject)
         {
             Camera cam = GetComponent<Camera>();
             cam.orthographicSize -= cam.orthographicSize * negSpeed;
         }
         else
         {
             if (fakeZoomParentObject != null)
                 fakeZoomParentObject.localScale += fakeZoomParentObject.localScale * negSpeed;
         }
         transform.position += (camToTarget * negSpeed);
     }
}
