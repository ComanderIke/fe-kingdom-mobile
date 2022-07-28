using UnityEngine;

namespace GameCamera
{
    [RequireComponent(typeof(CameraSystem))]
    public abstract class CameraMixin : MonoBehaviour
    {
        protected internal CameraSystem CameraSystem;
        private bool locked;

        public bool IsLocked()
        {
            return locked;
        }

        public CameraMixin Locked(bool value)
        {
            locked = value;
            return this;
        }
    }
}