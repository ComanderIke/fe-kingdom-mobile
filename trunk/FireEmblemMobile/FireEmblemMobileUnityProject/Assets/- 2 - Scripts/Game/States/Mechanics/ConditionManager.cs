using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameInput;

namespace Game.Mechanics
{
    public class ConditionManager
    {
        public bool CheckWin(List<Faction> Factions)
        {

            foreach (var p in Factions)
            {

                if (!p.IsPlayerControlled && !p.IsAlive())
                {
                    return true;
                }
            }

            return false;
        }
        public bool CheckLose(List<Faction> Factions)
        {

            foreach (var p in Factions)
            {
                if (p.IsPlayerControlled && !p.IsAlive())
                {
                    return true;
                }
            }

            return false;
        }
    }
}