using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class BirdMovingController : MonoBehaviour
    {
       [SerializeField] private LineRenderer lineRenderer;
        private int currentpositionIndex;
        private int nextPositionIndex = 1;
        private float time = 0;
       [SerializeField] private float speed = 1;
        void Start()
        {
        
        }

        void Update()
        {
            var nextPosition = lineRenderer.GetPosition(nextPositionIndex);
            time += Time.deltaTime*speed;
            transform.position = Vector3.Lerp(lineRenderer.GetPosition(currentpositionIndex), nextPosition, time);
            if (time >= 1)
            {
                time = 0;
                currentpositionIndex++;
                nextPositionIndex = currentpositionIndex + 1;
                if (currentpositionIndex >= lineRenderer.positionCount)
                {
                    currentpositionIndex = 0;
                    nextPositionIndex = 1;
                }
                if (nextPositionIndex >= lineRenderer.positionCount)
                {
                    nextPositionIndex = 0;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(lineRenderer.GetPosition(0), 1);
            Gizmos.DrawSphere(lineRenderer.GetPosition(lineRenderer.positionCount-1), 1);
        }
    }
}
