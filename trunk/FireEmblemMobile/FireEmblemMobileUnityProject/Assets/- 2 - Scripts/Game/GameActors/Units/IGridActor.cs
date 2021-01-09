using System.Collections.Generic;
using Game.Grid;

namespace Game.GameActors.Units
{
    public interface IGridActor
    {
        GridPosition GridPosition { get; }
        int MovementRage { get; }
        IEnumerable<int> AttackRanges { get; }
        int FactionId { get; }
        bool CanMoveOnTo(Tile tile);
        bool CanMoveThrough(IGridActor unit);
        bool IsEnemy(IGridActor unit);
        
        bool CanAttack(int x, int y);
    }
}