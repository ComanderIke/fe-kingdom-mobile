using System;
using System.Collections.Generic;
using Game.Mechanics;

namespace Game.AI
{
    public enum AttackResult
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
            int  xBonuses= x.GetTileDefenseBonuses() + x.GetTileAvoidBonuses()+x.GetTileSpeedBonuses();
            int yBonuses = y.GetTileDefenseBonuses() + y.GetTileAvoidBonuses()+y.GetTileSpeedBonuses();
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
            switch (x.AttackResult)
            {
                case AttackResult.Win:
                    return y.AttackResult == AttackResult.Win ? 0 : 1;
                case AttackResult.Draw when y.AttackResult == AttackResult.Win:
                    return -1;
                case AttackResult.Draw when y.AttackResult == AttackResult.Draw:
                    return 0;
                case AttackResult.Draw:
                    return 1;
                case AttackResult.Loss when y.AttackResult != AttackResult.Loss:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}