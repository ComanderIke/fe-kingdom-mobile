using Game.GameActors.Players;
using Game.GameActors.Units;

namespace Game.GameInput
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