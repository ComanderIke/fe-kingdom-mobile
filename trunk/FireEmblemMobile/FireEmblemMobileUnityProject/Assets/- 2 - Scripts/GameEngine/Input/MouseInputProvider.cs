using UnityEngine;

namespace GameEngine.Input
{
    public class MouseInputProvider : IInputProvider
    {
        public bool InputPressedUp()
        {
            return UnityEngine.Input.GetMouseButtonUp(0);
        }

        public bool InputPressedDown()
        {
            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        public bool InputPressed()
        {
            return  UnityEngine.Input.GetMouseButton(0);
        }

        public Vector3 InputPosition()
        {
            return UnityEngine.Input.mousePosition;
        }
    }
}