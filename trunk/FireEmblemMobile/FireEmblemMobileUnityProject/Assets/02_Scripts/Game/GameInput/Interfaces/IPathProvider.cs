using System.Collections.Generic;
using Game.GameActors.Units.Interfaces;
using Game.Grid.GridPathFinding;

namespace Game.GameInput.Interfaces
{
    public interface IPathProvider 
    {
        MovementPath GetPath(int startPosX, int startPosY, int targetPosX, int targetPosY, IGridActor character, bool toAdjacentPos, IEnumerable<int> statsAttackRanges);
    }
}