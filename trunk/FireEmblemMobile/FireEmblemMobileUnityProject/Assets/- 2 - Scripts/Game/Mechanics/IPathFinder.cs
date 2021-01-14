using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid.PathFinding;

namespace Game.Mechanics
{
    public interface IPathFinder
    {
        MovementPath FindPath(int x, int y, int x2, int y2, IGridActor unit, bool b, IEnumerable<int> range);
    }
}