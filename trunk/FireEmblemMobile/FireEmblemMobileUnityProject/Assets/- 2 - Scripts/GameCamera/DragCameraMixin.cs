using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameCamera
{
    public class DragCameraMixin : CameraMixin
    {
        private const float DRAG_SPEED = 0.08f;
        private bool drag;
        private Vector3 lastPosition;
        public List<string> excludeColliderTags = new List<string>();

        public void AddColliderTags(string tag)
        {
            excludeColliderTags.Add(tag);
        }
        private void Update()
        {
            if (Input.GetMouseButton(0) && drag)
            {
                var delta = Input.mousePosition - lastPosition;
                transform.Translate(-delta.x * Time.deltaTime * DRAG_SPEED, -delta.y * Time.deltaTime * DRAG_SPEED, 0);
                lastPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonDown(0))
            {
                var ray = CameraSystem.Camera.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
                if (hit.collider != null)
                    if (!excludeColliderTags.Contains(hit.collider.gameObject.tag))
                    {
                        lastPosition = Input.mousePosition;
                        drag = true;
                        //CameraSystem.DeactivateOtherMixins(this);
                    }
            }

           

            if (Input.GetMouseButtonUp(0))
            {
                CameraSystem.ActivateMixins();
                drag = false;
            }
        }
    }
}