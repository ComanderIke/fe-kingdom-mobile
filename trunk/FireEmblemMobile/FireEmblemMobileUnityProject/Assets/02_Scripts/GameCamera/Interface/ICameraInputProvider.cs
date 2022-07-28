using UnityEngine;

namespace GameEngine.Input
{
    public interface ICameraInputProvider
    {
        bool InputPressedUp();
        bool InputPressedDown();
        bool InputPressed();
        Vector3 InputPosition();
    }
}