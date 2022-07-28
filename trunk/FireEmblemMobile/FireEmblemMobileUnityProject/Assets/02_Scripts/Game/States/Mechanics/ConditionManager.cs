using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameInput;

namespace Game.Mechanics
{
    public abstract class ConditionManager
    {
        public abstract bool CheckWin();
    
        public abstract bool CheckLose();
  
    }
}