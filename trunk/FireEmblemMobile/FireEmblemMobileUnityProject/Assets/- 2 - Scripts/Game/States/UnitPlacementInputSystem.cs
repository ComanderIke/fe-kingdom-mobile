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
            offset = unitInputController.transform.position - eventData.pointerCurrentRaycast.worldPosition;
        }

        public void OnEndDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
            offset = Vector3.zero;
            var screenRay = Camera.main.ScreenPointToRay(eventData.position);
            // Perform Physics2D.GetRayIntersection from transform and see if any 2D object was under transform.position on drop.
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(screenRay);
            if (hit2D)
            {
                Debug.Log(hit2D.transform.gameObject.name);
                var dropComponent = hit2D.transform.gameObject.GetComponent<IDropHandler>();
                if (dropComponent != null)
                    dropComponent.OnDrop(eventData);
            }
            currentSelectedUnitController = null;
        }

        public void OnDrop(UnitInputController unitInputController, PointerEventData eventData)
        {
            if (unitInputController.unit != null)
            {
                Debug.Log("Swap Units");
                var tempX = unitInputController.unit.GridComponent.GridPosition.X;
                var tempY= unitInputController.unit.GridComponent.GridPosition.Y;
                unitInputController.unit.GridComponent.SetPosition(currentSelectedUnitController.unit.GridComponent.GridPosition.X,currentSelectedUnitController.unit.GridComponent.GridPosition.Y);
                //unitInputController.transform.position = new Vector3(currentSelectedUnitController.unit.GridComponent.GridPosition.Xtransform.position);
                currentSelectedUnitController.unit.GridComponent.SetPosition(tempX, tempY);
            }
        }
    }
}