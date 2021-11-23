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
            //Debug.Log("Touch Position: " + position);
            UnityEngine.EventSystems.PointerEventData pointer = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointer.position = position;
            List<UnityEngine.EventSystems.RaycastResult> raycastResults = new List<UnityEngine.EventSystems.RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointer, raycastResults);
 
            if (raycastResults.Count > 0)
            {
                foreach (var go in raycastResults)
                {
                    if (!go.gameObject.CompareTag("Grid"))
                    {
                        Debug.Log("RAYCAST NOT GRID: "+go.gameObject.name);
                        return true;
                    }
                    else
                    {
                        Debug.Log("RAYCAST GRID: "+go.gameObject.name);
                    }
                }
            }

            return false;
        }
        private bool IsPointerOverUIObject()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return true;
 
            for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
            {
                Touch touch = Input.GetTouch(touchIndex);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return true;
            }
 
            return false;
        }
        public void Update()
        {
            if(Input.touchCount ==1){
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) //&& !EventSystem.current.IsPointerOverGameObject())
                {

                    if (!CheckUIObjectsInPosition(touch.position))
                    {

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