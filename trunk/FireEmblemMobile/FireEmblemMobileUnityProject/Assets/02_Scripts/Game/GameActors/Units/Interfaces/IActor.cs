using Game.GameActors.Units.UnitState;

namespace Game.GameActors.Units.Interfaces
{
    public interface IActor
    {
        TurnStateManager TurnStateManager { get; set; }
        
    }
}