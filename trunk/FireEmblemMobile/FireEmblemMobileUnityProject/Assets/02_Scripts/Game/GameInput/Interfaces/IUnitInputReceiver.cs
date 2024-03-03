using Game.GameActors.Grid;
using Game.GameActors.Units.Interfaces;

namespace Game.GameInput.Interfaces
{
    public interface IUnitInputReceiver
    {
        void ActorDragEnded(IGridActor gridActor, int x, int y);
        void ActorDragged(IGridActor actor, int x, int y );
        void ActorClicked(IGridActor unit);
        void ActorDoubleClicked(IGridActor unit);
        void StartDraggingActor(IGridActor actor);
        void DraggedOverObject(IGridObject gridObject);
        void ActorLongHold(IGridActor unit);
    }
}