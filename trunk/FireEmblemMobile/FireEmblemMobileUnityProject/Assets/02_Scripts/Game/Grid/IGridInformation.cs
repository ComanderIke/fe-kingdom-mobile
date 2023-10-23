using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Grid;
using UnityEngine;

namespace Game.AI
{
    public interface IGridInformation
    {
        List<Vector2Int> GetMoveLocations(IGridActor unit);
        List<IAttackableTarget> GetAttackTargetsAtPosition(IGridActor unit, int locX, int locY);
        List<IAttackableTarget> GetSkillTargetsAtPosition(IGridActor unit, int locX, int locY);

        IGridObject GetGridObject(Vector2Int loc);
        Tile GetTile(int x, int y);
        Tile[,] GetTiles();
    }
}