using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostGrace
{
    public class BirdMovingController : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private SpriteRenderer spriteRenderer;

   [SerializeField] private int currentpositionIndex;
       [SerializeField] private int nextPositionIndex = 1;
        private float time = 0;
       [SerializeField] private float speed = 1;
        [SerializeField]private int direction = 1;
        [SerializeField]private Vector3 offset;
        void Start()
        {
        
        }

        void Update()
        {
            spriteRenderer.flipX = direction != 1;
            var nextPosition = lineRenderer.GetPosition(nextPositionIndex);
            time += Time.deltaTime*speed;
            transform.position = Vector3.Lerp(lineRenderer.GetPosition(currentpositionIndex)+offset, nextPosition+offset, time);
            
            if (time >= 1)
            {
                time = 0;
                currentpositionIndex+= direction;
                nextPositionIndex = currentpositionIndex + direction;
                if (nextPositionIndex >= lineRenderer.positionCount)
                {
                    currentpositionIndex=lineRenderer.positionCount-1;
                    nextPositionIndex =currentpositionIndex-1;
                    direction *= -1;
                }
                else if (nextPositionIndex < 0)
                {
                    direction *= -1;
                    currentpositionIndex = 0;
                    nextPositionIndex = 1;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(lineRenderer.GetPosition(0)+offset, 1);
            Gizmos.DrawSphere(lineRenderer.GetPosition(lineRenderer.positionCount-1)+offset, 1);
        }
    }
}
