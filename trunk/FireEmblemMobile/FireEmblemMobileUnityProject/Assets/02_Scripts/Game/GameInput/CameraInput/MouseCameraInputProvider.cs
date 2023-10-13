using LostGrace;
using Pathfinding;
using UnityEngine;

namespace GameEngine.Input
{
    public class MouseCameraInputProvider : ICameraInputProvider
    {
        public bool InputPressedUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0) && UnityEngine.Input.touchCount <= 1 && !InteractionBlocker.Instance.gameObject.activeSelf;
        }

        public bool InputPressedDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0) && UnityEngine.Input.touchCount <= 1&& !InteractionBlocker.Instance.gameObject.activeSelf;
        }

        public bool InputPressed()
        {
            return  UnityEngine.Input.GetMouseButton(0) && UnityEngine.Input.touchCount <= 1 && !InteractionBlocker.Instance.gameObject.activeSelf;
        }

        public Vector3 InputPosition()
        {
            return UnityEngine.Input.mousePosition;
        }
    }
}