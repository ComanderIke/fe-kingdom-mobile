﻿
using UnityEngine;


namespace GameCamera
{
    public class ViewOnGridMixin : CameraMixin
    {
       
        [SerializeField]
        private float maxZoom=7.0f;
        [SerializeField]
        private float minZoom=1.8f;
        [SerializeField]
        private float minX=-5;
        [SerializeField]
        private float maxX=18;
        [SerializeField]
        private float minY=-5;
        [SerializeField]
        private float maxY=12;

        private const float defaultOrthSize = 3.2f;
        public int zoomLevel;
        public int maxZoomLevel = 3;
        public float zoomSpeed=0.004f;
        private float extraBoundsPerZoomLevel = 2.0f;
        private float width;
        private float height;
        public void Construct(float width, float height, int zoomLevel)
        {
            this.zoomLevel = zoomLevel;
            this.width = width;
            this.height = height;
            SetBounds(width, height);
        }
        private void SetBounds(float width, float height)
        {
           
            minX = -CameraSystem.cameraData.cameraBoundsBorder.x-extraBoundsPerZoomLevel*zoomLevel;


            maxX = width + CameraSystem.cameraData.cameraBoundsBorder.x+extraBoundsPerZoomLevel*zoomLevel;
            minY = -CameraSystem.cameraData.cameraBoundsBorder.y-extraBoundsPerZoomLevel*zoomLevel;
            maxY = height + CameraSystem.cameraData.cameraBoundsBorder.y+extraBoundsPerZoomLevel*zoomLevel;
        }

        public void ToogleZoom()
        {
            zoomLevel++;
            if (zoomLevel >= maxZoomLevel)
                zoomLevel = 0;
            SetBounds(width, height);
        }
        private void LateUpdate()
        {
            // if (Input.touchCount == 2)
            // {
            //      Touch touch0, touch1;
            //      Vector2 touch0prevPos, touch1prevPos;
            //     // float distance;
            //      touch0 = Input.GetTouch(0);
            //      touch1 = Input.GetTouch(1);
            //      touch0prevPos = touch0.position - touch0.deltaPosition;
            //      touch1prevPos = touch1.position - touch1.deltaPosition;
            //      float prevTouchDeltaMag = (touch0prevPos - touch1prevPos).magnitude;
            //      float touchDeltaMag = (touch0.position-touch1.position).magnitude;
            //      float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;
            //
            //      CameraSystem.camera.orthographicSize += deltaMagDiff * zoomSpeed;
            //      CameraSystem.camera.orthographicSize = Mathf.Max(CameraSystem.camera.orthographicSize,minZoom);
            //      CameraSystem.camera.orthographicSize = Mathf.Min(CameraSystem.camera.orthographicSize,maxZoom);
            //      CameraSystem.uiCamera.orthographicSize = CameraSystem.camera.orthographicSize;
            // }
            CameraSystem.camera.orthographicSize = zoomLevel + defaultOrthSize;
            CameraSystem.uiCamera.orthographicSize = zoomLevel + defaultOrthSize;
            Vector3  topRight =  CameraSystem.camera.ScreenToWorldPoint(new Vector3( CameraSystem.camera.pixelWidth,  CameraSystem.camera.pixelHeight, -transform.position.z));
            Vector3  bottomLeft =  CameraSystem.camera.ScreenToWorldPoint(new Vector3(0,0,-transform.position.z));
       
            if(topRight.x > maxX)
            {
                transform.position = new Vector3(transform.position.x - (topRight.x - maxX), transform.position.y, transform.position.z);
            }
       
            if(topRight.y > maxY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (topRight.y - maxY), transform.position.z);
            }
       
            if(bottomLeft.x < minX)
            {
                transform.position = new Vector3(transform.position.x + (minX - bottomLeft.x), transform.position.y, transform.position.z);
            }
       
            if(bottomLeft.y < minY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (minY - bottomLeft.y), transform.position.z);
            }
           
        }
    }
}