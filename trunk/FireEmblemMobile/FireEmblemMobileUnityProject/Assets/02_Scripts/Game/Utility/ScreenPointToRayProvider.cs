using GameEngine.Tools;
using UnityEngine;

namespace Game.Utility
{
    public class ScreenPointToRayProvider : IRayProvider
    {
        private Camera camera;

        public ScreenPointToRayProvider(Camera camera)
        {
            this.camera = camera;
        }
        public Ray CreateRay(Vector3 mousePos)
        {
            return camera.ScreenPointToRay(mousePos);
        }
    }
}