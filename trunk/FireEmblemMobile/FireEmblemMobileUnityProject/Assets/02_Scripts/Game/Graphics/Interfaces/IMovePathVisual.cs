using System.Collections.Generic;
using UnityEngine;

namespace Game.Graphics
{
    public interface IMovePathVisual
    {
        void DrawMovementPath(List<Vector2Int> mousePath, int startX, int startY);
        void HideMovementPath();
    }
}