using System.Collections.Generic;
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
            if (blockDrag)
                return;
            if (CameraInputProvider.InputPressed())
            {
                DragPerformer.Drag(transform, CameraInputProvider.InputPosition());

            }

            if (CameraInputProvider.InputPressedDown())
            {
                var ray = RayProvider.CreateRay(CameraInputProvider.InputPosition());
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
    }
}
