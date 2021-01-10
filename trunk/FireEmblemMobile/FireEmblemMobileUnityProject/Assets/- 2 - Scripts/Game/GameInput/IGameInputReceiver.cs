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
        void DraggedOnActor(IGridActor gridActor);
        void DraggedOverActor(IGridActor gridActor);
        void StartDraggingActor(IGridActor actor);
        void ResetInput();
    }
}