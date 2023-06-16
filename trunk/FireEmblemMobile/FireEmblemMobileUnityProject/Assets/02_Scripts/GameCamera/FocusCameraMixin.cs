using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class FocusCameraMixin : CameraMixin
    {
        [SerializeField] private GameObject[] targets;
        private Vector3 _targetPosition;
        private float _targetSize;
        [SerializeField] private float _minimumOrthographicSize = 0.01f;
        [SerializeField][Min(1f)] private float spacingFactor = 1;
        [SerializeField] private bool lockYAxis = true;
        [SerializeField] private bool lockZAxis = true;
        [SerializeField] private float focusTime = 0.35f;
        private float time = 0;
       

        public void SetTargets(GameObject target)
        {
            SetTargets(new GameObject[]{target});
           
        }

        public void SetTargets(GameObject[] targets)
        {
            this.targets = targets;
            Debug.Log("SET TARGETS: "+targets.Length);
            Focus();
        }

       

        void Focus()
        {
            
            if (targets.TryGetBounds(out var bounds))
            {
                time = 0;
                Debug.Log("DOING FOCUS");
                _targetPosition = bounds.center;
                if (lockYAxis)
                    _targetPosition.y = CameraSystem.camera.transform.position.y;
                if(lockZAxis)
                    _targetPosition.z = CameraSystem.camera.transform.position.z;

                _targetSize = CalculateOrthographicSize(bounds);
            }
        }
        private void Update()
        {
           // Debug.Log("TARGETS: "+targets);
            


           if (targets!=null&&targets.Length != 0)
           {

                time += Time.deltaTime/focusTime;
                CameraSystem.camera.transform.position = Vector3.Slerp(CameraSystem.camera.transform.position, _targetPosition, time);
                if (time>=1)
                {
                    targets = null;
                }
                //CameraSystem.camera.orthographicSize = Mathf.Lerp(CameraSystem.camera.orthographicSize, _targetSize, 5 * Time.deltaTime);
            }
        }
       
      

       
       


        private float CalculateOrthographicSize(Bounds boundingBox)
        {
            var orthographicSize = CameraSystem.camera.orthographicSize;

            Vector2 min = boundingBox.min;
            Vector2 max = boundingBox.max;

            var width = (max - min).x * spacingFactor;
            var height = (max - min).y * spacingFactor;

            if (width > height)
            {
                orthographicSize = Mathf.Abs(width) / CameraSystem.camera.aspect / 2f;
            }
            else
            {
                orthographicSize = Mathf.Abs(height) / 2f;
            }

            return Mathf.Max(orthographicSize, _minimumOrthographicSize);
        }

        public void Construct()
        {
            _targetPosition = CameraSystem.camera.transform.position;
            _targetSize = CameraSystem.camera.orthographicSize;
            Debug.Log("Camera Target Pos: "+_targetPosition);
        }
    }
}