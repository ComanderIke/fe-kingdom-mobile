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
        
 
        bool CheckUIObjectsInPosition(Vector2 position)
        {
          
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            Debug.Log("Raycast Position: " + pointer.position);
 
            if (raycastResults.Count > 0)
            {
                Debug.Log("Results: " + raycastResults.Count);
                foreach (var go in raycastResults)
                {
                    if (!go.gameObject.CompareTag("Grid"))
                    {
                        Debug.Log("RAYCAST NOT GRID: "+go.gameObject.name);
                       
                    }
                    else
                    {
                        Debug.Log("RAYCAST GRID: "+go.gameObject.name);
                    }
                }
            }

            if (raycastResults.Count > 0)
                return true;

            return false;
        }
        // private bool IsPointerOverUIObject() {
        //     PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //     eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //     List<RaycastResult> results = new List<RaycastResult>();
        //     EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        //     return results.Count > 0;
        // }
        public void Update()
        {
            if(Input.touchCount ==1){
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) //&& !EventSystem.current.IsPointerOverGameObject())
                {

                    if (!CheckUIObjectsInPosition(touch.position))
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
                                    inputReceiver.GridClicked(x, y);
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