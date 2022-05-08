using System.Collections.Generic;
using __2___Scripts.Game.Utility;
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
            if(Input.touchCount ==1){
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) //&& !EventSystem.current.IsPointerOverGameObject())
                {

                    if (!UIClickChecker.CheckUIObjectsInPosition())
                    {
                        Debug.Log("NoUIClicked!");
                        if (!CameraSystem.IsDragging)
                        {

                            var gridPos = raycastManager.GetMousePositionOnGrid();
                            var x = (int)gridPos.x;
                            var y = (int)gridPos.y;
                            var hit = raycastManager.GetLatestHit();
                            Debug.Log("Check Clicked on Grid?");
                            if (IsClickOnGrid(hit))
                            {
                                Debug.Log("Player Input: Grid clicked: " + x + " " + y);
                                foreach (var inputReceiver in inputReceivers)
                                {
                                    inputReceiver.GridClicked(x, y);
                                }
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Began) //&& !EventSystem.current.IsPointerOverGameObject())
                {

                    if (!UIClickChecker.CheckUIObjectsInPosition())
                    {
                        Debug.Log("NoUIClicked!");
                        if (!CameraSystem.IsDragging)
                        {

                            var gridPos = raycastManager.GetMousePositionOnGrid();
                            var x = (int)gridPos.x;
                            var y = (int)gridPos.y;
                            var hit = raycastManager.GetLatestHit();

                            if (IsClickOnGrid(hit))
                            {
                                Debug.Log("Player Input: Grid clicked: " + x + " " + y);
                                foreach (var inputReceiver in inputReceivers)
                                {
                                    inputReceiver.GridClickedDown(x, y);
                                }
                            }
                        }
                    }
                }


            }
           
        }
        private bool IsClickOnGrid(RaycastHit2D hit)
        {
            
            return raycastManager.ConnectedLatestHit() && hit.collider.CompareTag(TagManager.GridTag);

        }

    }
}