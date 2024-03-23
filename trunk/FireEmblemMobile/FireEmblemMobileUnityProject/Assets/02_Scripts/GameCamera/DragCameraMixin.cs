using System.Collections.Generic;
using System.Security.Cryptography;
using Game.GameInput;
using GameEngine.Input;
using GameEngine.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCamera
{
    public class DragCameraMixin : CameraMixin
    {
        private IDragPerformer DragPerformer { get; set; }
        private IRayProvider RayProvider { get; set; }
        private IHitChecker HitChecker { get; set; }
        private ICameraInputProvider CameraInputProvider { get; set; }
        public static bool blockDrag = false;
        private bool normalDragActive = true;

        public void Construct(IDragPerformer dragPerformer, IRayProvider rayProvider, IHitChecker hitChecker,
            ICameraInputProvider cameraInputProvider)
        {
            DragPerformer = dragPerformer;
            RayProvider = rayProvider;
            HitChecker = hitChecker;
            CameraInputProvider = cameraInputProvider;
        }

        private void Update()
        {
            CameraSystem.IsDragging = DragPerformer.IsDragging;
            var inputPosition = CameraInputProvider.InputPosition();
            if (CameraInputProvider.InputPressed())
            {
                float xDragDirection = 0;
                float yDragDirection = 0;
                if (IsOnScreenEdge(inputPosition, out xDragDirection, out yDragDirection))
                {
                    Debug.Log("On Screen edge:" + inputPosition + " " + Screen.height + " " + Screen.width);
                    // if (!CheckUIObjectsInPosition())
                    // {
                    float speed = 0.018f;
                    transform.Translate(xDragDirection * speed, yDragDirection * speed, 0, Space.World);

                        // if (DragPerformer.HasDragStarted())
                        //     DragPerformer.Drag(transform, CameraInputProvider.InputPosition());
                        // else
                        // {
                        //     DragPerformer.StartDrag(transform, CameraInputProvider.InputPosition(), true);
                        // }

                        return;
                    // }
                }
            }

            if (blockDrag||!normalDragActive)
                return;
           
           
            if (CameraInputProvider.InputPressed())
            {
              
                DragPerformer.Drag(transform, CameraInputProvider.InputPosition());

            }

            if (CameraInputProvider.InputPressedDown())
            {
                
                var ray = RayProvider.CreateRay(inputPosition);
                if (HitChecker.CheckHit(ray))
                {
                    if (!CheckUIObjectsInPosition())
                    {
                        DragPerformer.StartDrag(transform, CameraInputProvider.InputPosition());
                    }
                }
                
            }
            

            if (CameraInputProvider.InputPressedUp())
            {
                DragPerformer.EndDrag(transform);
                CameraSystem.ActivateMixins();
            }
        }

        public bool IsOnScreenEdge(Vector3 screenPos, out float xDirection, out float yDirection)
        {
            float threshold = 100;
            xDirection = 0;
            yDirection = 0;
            Debug.Log(Mathf.Abs(screenPos.y - Screen.height) + " " + threshold);
            if (Mathf.Abs(screenPos.x - Screen.width) <= threshold)
                xDirection = 1;
            if (Mathf.Abs(screenPos.y - Screen.height) <= threshold)
                yDirection = 1;
            if (screenPos.x  <= threshold)
                xDirection = -1;
            if (screenPos.y  <= threshold)
                yDirection = -1;
            return xDirection!=0 || yDirection!=0;
        }

        public static bool CheckUIObjectsInPosition()
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            //Debug.Log("Raycast Position: " + pointer.position);

            if (raycastResults.Count > 0)
            {
                //Debug.Log("Results: " + raycastResults.Count);
                foreach (var go in raycastResults)
                {
                    if (!go.gameObject.CompareTag("Grid"))
                    {
                        //  Debug.Log("RAYCAST NOT GRID: "+go.gameObject.name);

                    }
                    else
                    {
                        //Debug.Log("RAYCAST GRID: "+go.gameObject.name);
                    }
                }
            }
           // Debug.Log("RAYCHECK: "+raycastResults.Count);
            if (raycastResults.Count > 0)
            {
                //    Debug.Log("UICLICKCHECKER RETURN TRUE");
                return true;
            }

            //Debug.Log("UICLICKCHECKER RETURN FALSE");
            return false;
        }

        
        public void ActivateNormalDrag()
        {
            normalDragActive = true;
        }
        public void DeactivateNormalDrag()
        {
            normalDragActive = false;
        }
    }
}
