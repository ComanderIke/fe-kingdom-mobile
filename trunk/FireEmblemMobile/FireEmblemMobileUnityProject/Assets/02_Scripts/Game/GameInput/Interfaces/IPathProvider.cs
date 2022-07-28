using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid.GridPathFinding;

namespace Game.GameInput
{
    public interface IPathProvider 
    {
        MovementPath GetPath(int startPosX, int startPosY, int targetPosX, int targetPosY, IGridActor character, bool toAdjacentPos, IEnumerable<int> statsAttackRanges);
    }
}