using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameCamera
{
    public class ViewOnGridMixin : CameraMixin
    {
        private int currentZoom = -1;
        [SerializeField]
        private float minX=-5;
        [SerializeField]
        private float maxX=18;
        [SerializeField]
        private float minY=-5;
        [SerializeField]
        private float maxY=12;
     
       

        [Range(0, 3)] public int zoom = 1;
        public float zoomSpeed=0.004f;
        private float startDistance;
        public void Construct(float width, float height, int zoom)
        {
            this.zoom = zoom;
            SetBounds(width, height);
        }
        private void SetBounds(float width, float height)
        {
           
            minX = -CameraSystem.cameraData.cameraBoundsBorder.x;


            maxX = width + CameraSystem.cameraData.cameraBoundsBorder.x;
            minY = -CameraSystem.cameraData.cameraBoundsBorder.y;
            maxY = height + CameraSystem.cameraData.cameraBoundsBorder.y;
        }

        private void Update()
        {
            if (Input.touchCount == 2)
            {
                 Touch touch0, touch1;
                 Vector2 touch0prevPos, touch1prevPos;
                // float distance;
                 touch0 = Input.GetTouch(0);
                 touch1 = Input.GetTouch(1);
                 touch0prevPos = touch0.position - touch0.deltaPosition;
                 touch1prevPos = touch1.position - touch1.deltaPosition;
                 float prevTouchDeltaMag = (touch0prevPos - touch1prevPos).magnitude;
                 float touchDeltaMag = (touch0.position-touch1.position).magnitude;
                 float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;

                 CameraSystem.camera.orthographicSize += deltaMagDiff * zoomSpeed;
                 CameraSystem.camera.orthographicSize = Mathf.Max(CameraSystem.camera.orthographicSize,1f);
                 CameraSystem.camera.orthographicSize = Mathf.Min(CameraSystem.camera.orthographicSize,5f);
                 CameraSystem.uiCamera.orthographicSize = CameraSystem.camera.orthographicSize;
               
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
            // if (currentZoom != zoom)
            // {
            //     currentZoom = zoom;
            //     var cam = CameraSystem.camera;
            //     switch (zoom)
            //     {
            //         case 0:
            //             cam.orthographicSize = 3f;
            //
            //             cam.transform.localPosition = new Vector3(5.33f, 3f, cam.transform.localPosition.z);
            //             break;
            //         case 1:
            //             cam.orthographicSize = 4f;
            //
            //             cam.transform.localPosition = new Vector3(7, 4f, cam.transform.localPosition.z);
            //             break;
            //         // case 2:
            //         //     cam.orthographicSize = 5f;
            //         //
            //         //     cam.transform.localPosition = new Vector3(7, 4f, cam.transform.localPosition.z);
            //         //     break;
            //         // case 3:
            //         //     cam.orthographicSize = 6f;
            //         //     cam.transform.localPosition = new Vector3(9, 5f, cam.transform.localPosition.z);
            //         //     break;
            //     }
            // }
        }
    }
}