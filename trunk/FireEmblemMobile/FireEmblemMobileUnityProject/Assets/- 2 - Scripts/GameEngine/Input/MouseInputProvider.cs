using UnityEngine;

namespace GameEngine.Input
{
    public class MouseInputProvider : IInputProvider
    {
        public bool InputPressedUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0) && UnityEngine.Input.touchCount <= 1;
        }

        public bool InputPressedDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0) && UnityEngine.Input.touchCount <= 1;
        }

        public bool InputPressed()
        {
            return  UnityEngine.Input.GetMouseButton(0) && UnityEngine.Input.touchCount <= 1;
        }

        public Vector3 InputPosition()
        {
            return UnityEngine.Input.mousePosition;
        }
    }
}