using UnityEngine;

namespace Assets.GameCamera
{
    [RequireComponent(typeof(CameraSystem))]
    public abstract class CameraMixin : MonoBehaviour
    {
        protected CameraSystem CameraSystem;
        private bool locked;

        private void Start()
        {
            CameraSystem = GetComponent<CameraSystem>();
        }

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