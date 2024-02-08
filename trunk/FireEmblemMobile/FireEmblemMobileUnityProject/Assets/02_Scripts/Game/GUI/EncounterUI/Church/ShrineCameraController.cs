using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class ShrineCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject pivot;

        [SerializeField] private float rotTime = 1.5f;
        [SerializeField] private List<float> rotations;

        private int currentRotation;

        private float currentRotationExact = 0;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void Rotate()
        {
            LeanTween.cancel(pivot);
            float currentRotY = transform.localRotation.eulerAngles.y;
            float diff = currentRotationExact + currentRotY;
            MyDebug.LogTest("Rotation: "+currentRotationExact+" "+diff);
            LeanTween.rotateAroundLocal(pivot, Vector3.up,diff, rotTime).setEaseInOutQuad();

        }

        public void RotateRight()
        {
            currentRotation--;
            if (currentRotation < 0)
                currentRotation = rotations.Count - 1;
            currentRotationExact -= 90;
            Rotate();
        }

        public void RotateLeft()
        {
            currentRotation++;
            if (currentRotation >rotations.Count - 1)
                currentRotation = 0;
            currentRotationExact += 90;
            Rotate();
        }
    }
}
