using Game.GameActors.Units;

namespace Game.Mechanics
{
    public interface ISelectableActor :IGridActor
    {
        UnitTurnState UnitTurnState { get;  }
        
    }
}