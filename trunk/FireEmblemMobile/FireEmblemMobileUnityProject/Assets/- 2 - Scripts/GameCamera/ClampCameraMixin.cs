using ICSharpCode.NRefactory.Ast;
using UnityEngine;

namespace GameCamera
{
    
    public class ClampCameraMixin : CameraMixin
    {
 
        private float maxX;
        private float maxY;
        private float minX;
        private float minY;

        public void Construct(int width, int height)
        {
            SetBounds(width, height);
        }

        private void LateUpdate()
        {
            var pos = transform.localPosition;
            if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
            {
                transform.localPosition = new Vector3(Mathf.Clamp(pos.x, minX, maxX),
                    Mathf.Clamp(pos.y, minY, maxY), pos.z);
            }
        }

        private void SetBounds(int width, int height)
        {
            
            minX = -CameraSystem.cameraData.CameraBoundsBorder;
            maxX = width + CameraSystem.cameraData.CameraBoundsBorder;
            minY = -CameraSystem.cameraData.CameraBoundsBorder;
            maxY = height + CameraSystem.cameraData.CameraBoundsBorder;
        }
    }
}