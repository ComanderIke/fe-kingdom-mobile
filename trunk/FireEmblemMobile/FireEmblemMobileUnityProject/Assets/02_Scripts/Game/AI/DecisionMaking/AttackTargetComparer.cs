using System;
using System.Collections.Generic;

namespace Game.AI
{
    public class AttackTargetComparer : IComparer<AIAttackTarget>
    {
        private CombatResultComparer combatResultComparer;

        public AttackTargetComparer()
        {
            combatResultComparer = new CombatResultComparer();
        }
        public int Compare(AIAttackTarget x, AIAttackTarget y)
        {
            int ret = combatResultComparer.Compare(x.CombatResult, y.CombatResult);
            if (ret == 0)
            {
                //Slot Order Compare?
            }
            
            return ret;
        }

       
    }
}