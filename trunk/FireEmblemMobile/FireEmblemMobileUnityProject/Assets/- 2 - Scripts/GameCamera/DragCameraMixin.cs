using Assets.Core;
using Assets.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Assets.GameCamera
{
    public class DragCameraMixin : CameraMixin
    {
        private const float DRAG_SPEED = 0.15f;
        private bool drag;
        private Vector3 lastPosition;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out var hit, Mathf.Infinity);
                if (hit.collider != null)
                    if (hit.collider.gameObject.tag != TagManager.UnitTag 
                        || hit.collider.gameObject.GetComponent<UnitInputController>().Unit.Faction.Id 
                            != GridGameManager.Instance.FactionManager.ActivePlayerNumber)
                    {
                        lastPosition = Input.mousePosition;
                        drag = true;
                        //CameraSystem.DeactivateOtherMixins(this);
                    }
            }

            if (Input.GetMouseButton(0) && drag)
            {
                var delta = Input.mousePosition - lastPosition;
                transform.Translate(-delta.x * Time.deltaTime * DRAG_SPEED, -delta.y * Time.deltaTime * DRAG_SPEED, 0);
                lastPosition = Input.mousePosition; 
            }

            if (Input.GetMouseButtonUp(0))
            {
                CameraSystem.ActivateMixins();
                drag = false;
            }
        }
    }
}