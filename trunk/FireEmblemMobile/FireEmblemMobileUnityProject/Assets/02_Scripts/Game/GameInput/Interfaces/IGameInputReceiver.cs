using Game.GameActors.Players;
using Game.GameActors.Units;

namespace Game.GameInput
{
    public interface IGameInputReceiver
    {
        void ClickedOnGrid(int x, int y);
        void DraggedOnGrid(int gridPosX, int gridPosY);
        void DraggedOverGrid(int gridPosX, int gridPosY);
        void ClickedOnActor(IGridActor unit);
        void DoubleClickedActor(IGridActor unit);
        void DraggedOnObject(IGridObject gridObject);
        void DraggedOverObject(IGridObject gridObject);
        void StartDraggingActor(IGridActor actor);
        void ResetInput();
        void UndoClicked();
        
        void ClickedDownOnGrid(int x, int y);
        void LongClickOnCharacter(IGridActor unit);
    }
}