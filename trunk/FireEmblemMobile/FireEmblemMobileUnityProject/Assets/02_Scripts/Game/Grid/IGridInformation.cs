using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public interface IGridInformation
    {
        List<Vector2Int> GetMoveLocations(IGridActor unit);
        List<IGridObject> GetAttackTargets(IGridActor unit, int locX, int locY);
    }
}