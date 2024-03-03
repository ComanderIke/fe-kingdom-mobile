using System.Collections.Generic;
using Game.GameActors.Grid;
using Game.GameActors.Units.Components;
using Game.GameActors.Units.OnGameObject;
using Game.GameActors.Units.UnitState;
using Game.GameActors.Units.UnitType;
using Game.Grid.Tiles;

namespace Game.GameActors.Units.Interfaces
{
    public interface IGridActor : IGridObject
    {
        GridActorComponent GetActorGridComponent();
        List<int> AttackRanges { get; }
        int MovementRange { get; }
        MoveType MoveType { get; set; }
        GameTransformManager GameTransformManager { get; set; }
        TurnStateManager TurnStateManager { get; set; }


        bool IsAlive();
        void SetGridPosition(Tile newTile, bool moveGameObject=true);
        void SetInternGridPosition(Tile tile);
        void SetToOriginPosition();
    }
}