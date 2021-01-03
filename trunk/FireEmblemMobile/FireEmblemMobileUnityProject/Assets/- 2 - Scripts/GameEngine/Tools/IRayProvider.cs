using UnityEngine;

namespace GameEngine.Tools
{
    public interface IRayProvider
    {
        Ray CreateRay(Vector3 mousePos);
    }
}