
    using UnityEngine;

    namespace Game.Utility
    {
        /// <summary>
        /// Add this class to an object (usually a sprite) and it'll face the camera at all times
        /// </summary>
        public class MyBillboard : MonoBehaviour
        {
            /// the camera we're facing
            [SerializeField] private Camera MainCamera;
            /// whether or not this object should automatically grab a camera on start
        

            protected GameObject _parentContainer;
            private Transform _transform;

            /// <summary>
            /// On awake we grab a camera if needed, and nest our object
            /// </summary>
            protected virtual void Awake()
            {
                _transform = transform;
            
            }
        
       

            /// <summary>
            /// On update, we change our parent container's rotation to face the camera
            /// </summary>
            protected virtual void Update()
            {
                Vector3 v = MainCamera.transform.position - transform.position;
                v.x = v.z = 0.0f;
                transform.LookAt(MainCamera.transform.position-v);
            }
        }
    }
