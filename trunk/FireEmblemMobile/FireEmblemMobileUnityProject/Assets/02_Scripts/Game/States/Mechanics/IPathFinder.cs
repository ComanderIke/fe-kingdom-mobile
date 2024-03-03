using Game.GameActors.Units.Interfaces;
using Game.Grid.GridPathFinding;

namespace Game.States.Mechanics
{
    public interface IPathFinder
    {
        MovementPath FindPath(int x, int y, int x2, int y2, IGridActor unit);
    }
}