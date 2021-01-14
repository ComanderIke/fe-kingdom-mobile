using Game.GameActors.Units;
using UnityEngine;

namespace Game.Mechanics
{
    public interface ITileChecker
    {
        int GetWidth();
        int GetHeight();
        bool CheckField(int x, int y, IGridActor unit, int range);
        bool IsTileAccessible(int x, int y, IGridActor unit);
        bool IsTileFree(int x, int y);
        bool IsValidLocation(IGridActor unit, int sx, int sy, int xp, int yp, bool isAdjacent);
    }
}