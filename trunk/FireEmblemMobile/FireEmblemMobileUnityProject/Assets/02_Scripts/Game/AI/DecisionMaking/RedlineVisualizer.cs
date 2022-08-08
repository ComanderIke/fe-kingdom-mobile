using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.Grid.GridPathFinding;
using UnityEngine;

namespace Game.AI
{
    public class RedlineVisualizer : MonoBehaviour
    {
        //[SerializeField]Material redlineMaterial;
        [SerializeField]GameObject redlinePrefab;
        [SerializeField]GameObject blueLinePrefab;

        List<GameObject> instantiatedLines;

        private void Start()
        {
            instantiatedLines = new List<GameObject>();
        }

        public void ShowRed(Vector3 startPosition, Vector3 endPosition)
        {
            Show(redlinePrefab, startPosition, endPosition);
        }
        public void ShowBlue(Vector3 startPosition, Vector3 endPosition)
        {
            
            Show(blueLinePrefab, startPosition, endPosition);
          
        }

        private void Show(GameObject prefab, Vector3 startPosition, Vector3 endPosition)
        {
            var line = Instantiate(prefab, transform);
            var lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
            instantiatedLines.Add(line);
        }

        public void HideAll()
        {
            for (int i = instantiatedLines.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(instantiatedLines[i]);
            }
            instantiatedLines.Clear();
        }

        public void ShowRedPath( MovementPath closestTargetPath)
        {
            ShowPath(redlinePrefab, closestTargetPath);
        }
        private void ShowPath(GameObject prefab, MovementPath closestTargetPath)
        {
            var line = Instantiate(prefab, transform);
            var lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.positionCount = closestTargetPath.GetLength();
            int cnt = instantiatedLines.Count();
            for (int i = 0; i < closestTargetPath.GetLength(); i++)
            {
                var step = closestTargetPath.GetStep(i);
                lineRenderer.SetPosition(i, new Vector3(step.GetX()+0.5f-0.02f*cnt,step.GetY()+0.5f-0.02f*cnt, 2.0f));
            }
            
            instantiatedLines.Add(line);
        }
    }
}