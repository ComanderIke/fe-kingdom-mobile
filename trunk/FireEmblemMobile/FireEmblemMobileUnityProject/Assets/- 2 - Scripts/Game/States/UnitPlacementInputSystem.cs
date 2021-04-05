using System;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.States
{
    public class UnitPlacementInputSystem : IUnitTouchInputReceiver
    {
        private Vector3 offset;
        private UnitInputController currentSelectedUnitController;
        public Action<Unit, Unit> unitDroppedOnOtherUnit;
        public void OnMouseEnter(UnitInputController unitInputController)
        {
            
        }

        public void OnMouseDrag(UnitInputController unitInputController)
        {
            currentSelectedUnitController = unitInputController;

            // unitInputController.transform += 
        }

        public void OnMouseDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
           
            currentSelectedUnitController = unitInputController;

            unitInputController.transform.position=offset+ eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnMouseDown(UnitInputController unitInputController)
        {
            currentSelectedUnitController = unitInputController;
        }

        public void OnMouseUp(UnitInputController unitInputController)
        {
           
            //currentSelectedUnitController = null;
        }

        public void OnBeginDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
            currentSelectedUnitController = unitInputController;
            unitInputController.boxCollider.enabled = false;
            offset = unitInputController.transform.position - eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnEndDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
            offset = Vector3.zero;
            var screenRay = Camera.main.ScreenPointToRay(eventData.position);
            // Perform Physics2D.GetRayIntersection from transform and see if any 2D object was under transform.position on drop.
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(screenRay);
            unitInputController.boxCollider.enabled = true;
            if (hit2D)
            {
                var dropComponent = hit2D.transform.gameObject.GetComponent<IDropHandler>();
                if (dropComponent != null)
                {
                    dropComponent.OnDrop(eventData);
                    return;
                }

            }
            unitInputController.unit.GridComponent.ResetPosition();
            currentSelectedUnitController = null;
        }

        public void OnDrop(UnitInputController unitInputController, PointerEventData eventData)
        {
            if (unitInputController.unit != null&&currentSelectedUnitController!=null && unitInputController.unit!=currentSelectedUnitController.unit)
            {
                unitDroppedOnOtherUnit?.Invoke(unitInputController.unit, currentSelectedUnitController.unit);
                
            }
        }
    }
}