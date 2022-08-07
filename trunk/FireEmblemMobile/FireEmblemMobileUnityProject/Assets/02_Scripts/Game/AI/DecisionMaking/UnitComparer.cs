using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public class UnitComparer : IComparer<IAIAgent>
    {
        public int Compare(IAIAgent x, IAIAgent y)
        {
            if (x == null || y == null)
            {
                Debug.Log("Compare: Unit should not be null!");
                return 0;
            }

            int compare = CompareAttackRanges(x, y);
            if (compare == 0)
            {
                compare = CompareDistanceToClosestEnemy(x, y);
            }
            if (compare == 0)
            {
                compare = CompareSmallestXYPosition(x, y);
            }
            return compare;

        }

        private int CompareSmallestXYPosition(IAIAgent x, IAIAgent y)
        {
            if (x.GridComponent.GridPosition.X > y.GridComponent.GridPosition.X)
                return 1;
            if (x.GridComponent.GridPosition.X < y.GridComponent.GridPosition.X)
                return -1;
            if (x.GridComponent.GridPosition.Y > y.GridComponent.GridPosition.Y)
                return 1;
            if (x.GridComponent.GridPosition.Y < y.GridComponent.GridPosition.Y)
                return -1;
            return 0;
        }
        private int CompareDistanceToClosestEnemy(IAIAgent x, IAIAgent y)
        {
            var xDist = x.AIComponent.DistanceToClosestEnemy();
            var yDist = y.AIComponent.DistanceToClosestEnemy();
            if(xDist==-1 || yDist ==-1)
                Debug.Log("No Distance Found Targets = null" +x.GridComponent.GridPosition+" "+y.GridComponent.GridPosition);
            if (xDist > yDist)
                return -1;
            return xDist < yDist ? 1 : 0;
        }
        private int CompareAttackRanges(IAIAgent x, IAIAgent y)
        {
            bool xIsMelee = x.BattleComponent.BattleStats.IsMeleeOnly();
            bool yIsMelee = y.BattleComponent.BattleStats.IsMeleeOnly();
            if (xIsMelee && yIsMelee)
                return 0;
            else if (xIsMelee)
                return 1;
            else if (yIsMelee)
                return -1;
            else
            {
                bool xIsRange = x.BattleComponent.BattleStats.IsRangeOnly();
                bool yIsRange = y.BattleComponent.BattleStats.IsRangeOnly();
                if (xIsRange && yIsRange)
                    return 0;
                else if (xIsRange)
                    return -1;
                else if (yIsRange)
                    return 1;
                return 0;
            }
        }
    }
}