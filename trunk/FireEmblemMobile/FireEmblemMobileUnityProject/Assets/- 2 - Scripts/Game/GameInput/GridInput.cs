using System.Collections.Generic;
using Game.GameActors.Units.OnGameObject;
using Game.Manager;
using GameCamera;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameInput
{
    public class GridInput
    {
        private RaycastManager raycastManager;

        private List<IGridInputReceiver> inputReceivers;
        

        public GridInput()
        {
            inputReceivers = new List<IGridInputReceiver>();
            raycastManager = new RaycastManager();
        }

        public void RegisterInputReceiver(IGridInputReceiver inputReceiver)
        {
            if (!inputReceivers.Contains(inputReceiver))
                inputReceivers.Add(inputReceiver);
        }

        public void Update()
        {
            if (Input.GetMouseButtonUp(0) && Input.touchCount <= 2 && !EventSystem.current.IsPointerOverGameObject())
            {
                
                if(!CameraSystem.IsDragging)
                    CheckClickOnGrid();
            }
        }

        private void CheckClickOnGrid()
        {
            var gridPos = raycastManager.GetMousePositionOnGrid();
            var hit = raycastManager.GetLatestHit();
            var x = (int) gridPos.x;
            var y = (int) gridPos.y;
            if (raycastManager.ConnectedLatestHit() && hit.collider.CompareTag(TagManager.GridTag))
            {
                //ResetDrag();
                Debug.Log("Player Input: Grid clicked: " + x + " " + y);
                foreach (var inputReceiver in inputReceivers)
                {
                    inputReceiver.GridClicked(x, y);
                }
            }
        }
    }
}