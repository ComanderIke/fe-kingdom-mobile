﻿using UnityEngine;

namespace GameCamera
{
    
    public class ClampCameraMixin : CameraMixin
    {
 
        private float maxX;
        private float maxY;
        private float minX;
        private float minY;
        private float width;
        private float height;

        public void Construct(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        private void LateUpdate()
        {
            SetBounds(width, height);
            var pos = transform.localPosition;
            if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
            {
                transform.localPosition = new Vector3(Mathf.Clamp(pos.x, minX, maxX),
                    Mathf.Clamp(pos.y, minY, maxY), pos.z);
            }
        }

        private void SetBounds(float width, float height)
        {
            var orthographicSize = CameraSystem.camera.orthographicSize;
            var aspect = CameraSystem.camera.aspect;
            var horizontalDelta = orthographicSize * 2 * aspect - orthographicSize * 2 * 1920/1080;
            if (CameraSystem.cameraData.cameraBoundsBorder.x == 0)
            {
                minX = 0;
            }
            else
            {
                minX = -CameraSystem.cameraData.cameraBoundsBorder.x + horizontalDelta / 2;
            }

            maxX = width + CameraSystem.cameraData.cameraBoundsBorder.x - orthographicSize * 2 * aspect + horizontalDelta / 2;
            minY = -CameraSystem.cameraData.cameraBoundsBorder.y;
            maxY = height + CameraSystem.cameraData.cameraBoundsBorder.y- orthographicSize * 2;
        }
    }
}