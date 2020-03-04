using Assets.GameResources;
using UnityEngine;

namespace Assets.GameCamera
{
    public class ClampCameraMixin : CameraMixin
    {
        private new Camera camera;
        private CameraData cameraSettings;
        private int gridHeight;
        private int gridWidth;
        private float horizontalExtent;
        private float maxX;
        private float maxY;
        private float minX;
        private float minY;
        private float referenceHeight;
        private float uiHeight;
        private float verticalExtent;

        private void Start()
        {
            camera = Camera.main;
            cameraSettings = DataScript.Instance.CameraData;
            UpdateBounds();
        }

        private void LateUpdate()
        {
            if (camera.orthographicSize != verticalExtent) UpdateBounds();

            if (transform.localPosition.x < minX ||
                transform.localPosition.x > maxX ||
                transform.localPosition.y < minY ||
                transform.localPosition.y > maxY)
                transform.localPosition = new Vector3(
                    Mathf.Clamp(transform.localPosition.x, minX, maxX),
                    Mathf.Clamp(transform.localPosition.y, minY, maxY),
                    transform.localPosition.z);
        }

        private void UpdateBounds()
        {
            if (cameraSettings.CameraBoundsBorder > 0)
            {
                //Debug.Log("UpdateCameraBounds: " + uiHeight + " " + referenceHeight);
                verticalExtent = camera.orthographicSize;
                horizontalExtent = verticalExtent * Screen.width / Screen.height;
                float verticalExtentWithoutGui = verticalExtent *
                                                 ((Screen.height - uiHeight * Screen.height / referenceHeight) /
                                                  Screen
                                                      .height); //GUI takes some space1385 are the number of pixels for NonGUI
                minX = -cameraSettings.CameraBoundsBorder;
                maxX = gridWidth - 2 * horizontalExtent + cameraSettings.CameraBoundsBorder;
                minY = -cameraSettings.CameraBoundsBorder;
                maxY = gridHeight - 2 * verticalExtentWithoutGui - 1 + 1 + cameraSettings.CameraBoundsBorder;
                //Debug.Log("MinX: " + minX + " " + maxX + " " + minY + " " + maxY);
            }
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

        public ClampCameraMixin UiHeight(int uiHeight)
        {
            this.uiHeight = uiHeight;
            return this;
        }

        public ClampCameraMixin ReferenceHeight(int referenceHeight)
        {
            this.referenceHeight = referenceHeight;
            return this;
        }
    }
}