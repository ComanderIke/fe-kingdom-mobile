using Game.GameActors.Grid;
using Game.GameActors.Units.Interfaces;

namespace Game.GameInput.Interfaces
{
    public interface IGameInputReceiver
    {
        void ClickedOnGrid(int x, int y, bool resetPosition=false);
        void DraggedOnGrid(int gridPosX, int gridPosY);
        void DraggedOverGrid(int gridPosX, int gridPosY);
        void ClickedOnActor(IGridActor unit);
        void DoubleClickedActor(IGridActor unit);
        void DraggedOnObject(IGridObject gridObject);
        void DraggedOverObject(IGridObject gridObject);
        void StartDraggingActor(IGridActor actor);
        void ResetInput(bool drag=false, bool move=false);
        void UndoClicked();
        
        void ClickedDownOnGrid(int x, int y);
        void LongClickOnCharacter(IGridActor unit);
    }
}