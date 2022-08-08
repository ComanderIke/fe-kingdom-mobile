using System;
using System.Collections.Generic;
using Game.Mechanics;

namespace Game.AI
{
    public enum BattleResult
    {
        Win,
        Draw,
        Loss
    }
    public class CombatResultComparer : IComparer<ICombatResult>
    {
        public int Compare(ICombatResult x, ICombatResult y)
        {
            int ret = 0;
            ret= CompareBattleResult(x, y);
            if (ret == 0)
            {
                ret=CompareDamageRatio(x, y);
            }

            if (ret == 0)
            {
                ret=CompareDefenseTiles(x, y);
            }
            return ret;
        }

        private int CompareDefenseTiles(ICombatResult x, ICombatResult y)
        {
            int  xBonuses= x.GetTileDefenseBonuses() + x.GetTileAvoidBonuses();
            int yBonuses = y.GetTileDefenseBonuses() + y.GetTileAvoidBonuses();
            if ( xBonuses> yBonuses)
            {
                return 1;
            }
            if (xBonuses < yBonuses)
            {
                return -1;
            }

            return 0;
        }
        private int CompareDamageRatio(ICombatResult x, ICombatResult y)
        {
            if (x.GetDamageRatio() > y.GetDamageRatio())
            {
                return 1;
            }
            if (x.GetDamageRatio() < y.GetDamageRatio())
            {
                return -1;
            }

            return 0;
        }

        private int CompareBattleResult(ICombatResult x, ICombatResult y)
        {
            switch (x.BattleResult)
            {
                case BattleResult.Win:
                    return y.BattleResult == BattleResult.Win ? 0 : 1;
                case BattleResult.Draw when y.BattleResult == BattleResult.Win:
                    return -1;
                case BattleResult.Draw when y.BattleResult == BattleResult.Draw:
                    return 0;
                case BattleResult.Draw:
                    return 1;
                case BattleResult.Loss when y.BattleResult != BattleResult.Loss:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}