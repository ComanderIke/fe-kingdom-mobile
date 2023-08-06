using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameInput
{
    public class LastInputPositionManager
    {
        private List<Vector2> lastPositions;

        public LastInputPositionManager()
        {
            lastPositions = new List<Vector2>();
        }
        public void StoreLatestValidPosition(int x, int y, int maxCount)
        {
            lastPositions.Add(new Vector2(x, y));

            if (lastPositions.Count > maxCount)
                lastPositions.RemoveAt(0);
        }
        public Vector2 GetLastAttackPosition(IGridActor c, int xAttack, int zAttack)
        {
            for (int i = c.AttackRanges.Count() - 1; i >= 0; i--) //Prioritize Range Attacks
            for (int j = lastPositions.Count - 1; j >= 0; j--)
                if (GetDelta(lastPositions[j], new Vector2(xAttack, zAttack)) == c.AttackRanges.ElementAt(i))
                    return lastPositions[j];

            return new Vector2(-1, -1);
        }
        private int GetDelta(Vector2 v, Vector2 v2)
        {
            var xDiff = (int) Mathf.Abs(v.x - v2.x);
            var zDiff = (int) Mathf.Abs(v.y - v2.y);
            return xDiff + zDiff;
        }

        public Vector2 GetLastValidPosition()
        {
            return lastPositions.Last();
        }
    }
}