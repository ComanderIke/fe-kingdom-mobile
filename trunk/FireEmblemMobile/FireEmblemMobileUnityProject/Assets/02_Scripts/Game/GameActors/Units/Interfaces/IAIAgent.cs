using System.Collections.Generic;
using Game.AI;
using Game.GameActors.Players;

namespace Game.GameActors.Units
{
    public interface IAIAgent :IGridActor
    {
        AIComponent AIComponent { get; }
        
        BattleComponent BattleComponent { get; }
        
    }
}