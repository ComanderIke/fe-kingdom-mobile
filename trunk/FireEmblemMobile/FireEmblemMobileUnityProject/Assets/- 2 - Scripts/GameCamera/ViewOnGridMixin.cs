using UnityEngine;
using UnityEngine.Serialization;

namespace GameCamera
{
    public class ViewOnGridMixin : CameraMixin
    {
        private int currentZoom = -1;

        [Range(0, 3)] public int zoom = 0;
        

        private void Update()
        {
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
  
                        cam.transform.localPosition = new Vector3(6, 3f, cam.transform.localPosition.z);
                        break;
                    case 2:
                        cam.orthographicSize = 5f;
    
                        cam.transform.localPosition = new Vector3(7, 4f, cam.transform.localPosition.z);
                        break;
                    case 3:
                        cam.orthographicSize = 6f;
                        cam.transform.localPosition = new Vector3(9, 5f, cam.transform.localPosition.z);
                        break;
                }
            }
        }
    }
}