using System.Collections.Generic;
using System.Numerics;

namespace Game.Grid
{
    public class GridSessionData
    {
        public List<Vector2> validPositions = new List<Vector2>();
        public List<Vector2> validAttackTargets = new List<Vector2>();

        public GridSessionData()
        {
            validPositions = new List<Vector2>();
            validAttackTargets = new List<Vector2>();
        }
        public bool IsMoveableAndActive(int x, int y)
        {
            return validPositions.Contains(new Vector2(x, y));
        }

        public bool IsAttackableAndActive(int x, int y)
        {
            return validAttackTargets.Contains(new Vector2(x, y));
        }

        public void Clear()
        {
            validPositions.Clear();
            validAttackTargets.Clear();
        }
    }
}