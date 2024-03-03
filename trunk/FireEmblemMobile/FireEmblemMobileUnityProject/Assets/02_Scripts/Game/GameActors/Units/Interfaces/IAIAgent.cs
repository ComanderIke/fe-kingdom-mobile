using Game.AI.UnitSpecific;
using Game.GameActors.Units.Components;

namespace Game.GameActors.Units.Interfaces
{
    public interface IAIAgent :IGridActor
    {
        AIComponent AIComponent { get; }
        
        BattleComponent BattleComponent { get; }

    }
}