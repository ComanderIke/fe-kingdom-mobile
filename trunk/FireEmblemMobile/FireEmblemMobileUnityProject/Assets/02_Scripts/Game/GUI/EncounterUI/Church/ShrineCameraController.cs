using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LostGrace
{
    public class ShrineCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject pivot;

        [SerializeField] private float startRotTime = 1.5f;
        private float rotTime = 0;
        [SerializeField] private List<float> rotations;
        public Queue<float> rotationQueue;

        private int currentRotation;

        private float currentRotationExact = 0;
        // Start is called before the first frame update
        void Start()
        {
            rotationQueue = new Queue<float>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void Rotate(bool freshStart=false)
        {
            if (rotationQueue.Count == 0)
                return;
            LeanTween.cancel(pivot);
            MyDebug.LogTest("RotTime: "+rotTime);
            var easeType = rotationQueue.Count == 1 ? LeanTweenType.easeOutQuad : LeanTweenType.notUsed;
            if (freshStart)
                easeType = LeanTweenType.easeInOutQuad;
            rotTime = startRotTime/rotationQueue.Count;
            LeanTween.rotateLocal(pivot, new Vector3(0,rotationQueue.Peek(),0), rotTime).setEase(easeType).setOnComplete(()=>
            {
                rotationQueue.Dequeue();
                if (rotationQueue.Count() != 0)
                {
                    Rotate();
                }
            });

        }

        public void RotateRight()
        {
            currentRotation--;
            if (currentRotation < 0)
                currentRotation = rotations.Count - 1;
            rotationQueue.Enqueue(rotations[currentRotation]);
            Rotate(rotationQueue.Count==1);
        }

        public void RotateLeft()
        {
            currentRotation++;
            if (currentRotation >rotations.Count - 1)
                currentRotation = 0;
            rotationQueue.Enqueue(rotations[currentRotation]);
            
            //if(rotationQueue.Count==1)
            Rotate(rotationQueue.Count==1);
        }

        public void JumpTo(int selectedGod)
        {
            if (selectedGod % rotations.Count() == currentRotation&&selectedGod!=currentRotation)
                selectedGod += 2;
            currentRotation = selectedGod % rotations.Count();
            rotationQueue.Clear();
            LeanTween.cancel(pivot);
            rotationQueue.Enqueue(rotations[currentRotation]);
            
            //if(rotationQueue.Count==1)
            Rotate(rotationQueue.Count==1);
        }
    }
}
