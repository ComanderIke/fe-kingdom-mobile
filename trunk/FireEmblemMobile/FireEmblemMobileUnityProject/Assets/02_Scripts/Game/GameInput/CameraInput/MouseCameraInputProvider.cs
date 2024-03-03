using Game.Utility;
using GameEngine.Input;
using UnityEngine;

namespace Game.GameInput.CameraInput
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