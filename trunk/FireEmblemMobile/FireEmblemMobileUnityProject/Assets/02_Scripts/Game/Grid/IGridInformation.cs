using System.Collections.Generic;
using Game.GameActors.Grid;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units.Interfaces;
using Game.Grid.Tiles;
using UnityEngine;

namespace Game.Grid
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