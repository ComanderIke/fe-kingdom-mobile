using System.Collections.Generic;
using UnityEngine;

namespace Game.Graphics.Interfaces
{
    public interface IMovePathVisual
    {
        void DrawMovementPath(List<Vector2Int> mousePath, int startX, int startY);
        void HideMovementPath();

        void Reset();
    }
}