using Game.GameInput.UnitInput;
using Game.LevelDesign;

namespace Game.Systems
{
    public interface IUnitTouchInputReceiver
    {
        // void OnMouseEnter(UnitInputController unitInputController);
        void OnMouseDrag(UnitInputController unitInputController);
        
        // void OnMouseDrag(UnitInputController unitInputController, PointerEventData eventData);
        void OnMouseDown(UnitInputController unitInputController);
        void OnMouseUp(UnitInputController unitInputController);
        // void OnBeginDrag(UnitInputController unitInputController, PointerEventData eventData);
        // void OnEndDrag(UnitInputController unitInputController, PointerEventData eventData);
        void OnDrop(UnitInputController unitInputController);
        void OnDrop(StartPosition startPosition);
        void OnMouseHold(UnitInputController unitInputController);
    }
}