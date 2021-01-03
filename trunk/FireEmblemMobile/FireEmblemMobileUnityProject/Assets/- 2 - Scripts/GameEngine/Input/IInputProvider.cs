using UnityEngine;

namespace GameEngine.Input
{
    public interface IInputProvider
    {
        bool InputPressedUp();
        bool InputPressedDown();
        bool InputPressed();
        Vector3 InputPosition();
    }
}