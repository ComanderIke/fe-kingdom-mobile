using System;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Game.States
{
    public class UnitPlacementInputSystem : IUnitTouchInputReceiver
    {
        private Vector3 offset;
        private UnitInputController currentSelectedUnitController;
        public Action<Unit, Unit> unitDroppedOnOtherUnit;
        public Action<Unit, StartPosition> unitDroppedOnStartPos;
        public bool active = true;

        public void OnMouseEnter(UnitInputController unitInputController)
        {
            
        }

        public void OnMouseDrag(UnitInputController unitInputController)
        {
            if (!active)
                return;
            currentSelectedUnitController = unitInputController;
            //unitInputController.transform.position=offset+ eventData.pointerCurrentRaycast.worldPosition;
        }

      

        public void OnMouseDown(UnitInputController unitInputController)
        {
            if (!active)
                return;
            currentSelectedUnitController = unitInputController;
        }

        public void OnMouseUp(UnitInputController unitInputController)
        {
            if (!active)
                return;
            var screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(screenRay);
            unitInputController.boxCollider.enabled = true;
            if (hit2D)
            {
                var dropComponent = hit2D.transform.gameObject.GetComponent<IMyDropHandler>();
                if (dropComponent != null)
                {
                    dropComponent.OnDrop();
                    return;
                }

            }
            Debug.Log("End Drag Reset");
            unitInputController.unit.GridComponent.ResetPosition();
            currentSelectedUnitController = null;
            //currentSelectedUnitController = null;
        }

        // public void OnBeginDrag(UnitInputController unitInputController)
        // {
        //     if (!active)
        //         return;
        //     Debug.Log("Begin Drag");
        //     currentSelectedUnitController = unitInputController;
        //     unitInputController.boxCollider.enabled = false;
        //     offset = unitInputController.transform.position - eventData.pointerCurrentRaycast.worldPosition;
        // }

        // public void OnEndDrag(UnitInputController unitInputController)
        // {
        //     if (!active)
        //         return;
        //     Debug.Log("End Drag");
        //     offset = Vector3.zero;
        //     var screenRay = Camera.main.ScreenPointToRay(eventData.position);
        //     // Perform Physics2D.GetRayIntersection from transform and see if any 2D object was under transform.position on drop.
        //     RaycastHit2D hit2D = Physics2D.GetRayIntersection(screenRay);
        //     unitInputController.boxCollider.enabled = true;
        //     if (hit2D)
        //     {
        //         var dropComponent = hit2D.transform.gameObject.GetComponent<IMyDropHandler>();
        //         if (dropComponent != null)
        //         {
        //             dropComponent.OnDrop();
        //             return;
        //         }
        //
        //     }
        //     Debug.Log("End Drag Reset");
        //     unitInputController.unit.GridComponent.ResetPosition();
        //     currentSelectedUnitController = null;
        // }
        public void OnDrop(StartPosition startPosition)
        {
            if (!active)
                return;
          
            if (startPosition.Actor== null&&currentSelectedUnitController!=null )
            {
                Debug.Log("Unit dropped on Start Pos Successfully");
                unitDroppedOnStartPos?.Invoke( currentSelectedUnitController.unit, startPosition);
                
            }
            else if (currentSelectedUnitController != null && startPosition.Actor !=null && startPosition.Actor == currentSelectedUnitController.unit)
            {
                Debug.Log("REset Position after Drop");
                currentSelectedUnitController.unit.GridComponent.ResetPosition();
            
            }
            Debug.Log(startPosition.Actor.name+" "+currentSelectedUnitController.unit.name);
        }

        public void OnMouseHold(UnitInputController unitInputController)
        {
            
        }

        public void OnDrop(UnitInputController unitInputController)
        {
            if (!active)
                return;
            if (unitInputController.unit != null&&currentSelectedUnitController!=null)
            {
                if (unitInputController.unit != currentSelectedUnitController.unit)
                {
                    unitDroppedOnOtherUnit?.Invoke(unitInputController.unit, currentSelectedUnitController.unit);
                }

            }
        }
    }
}