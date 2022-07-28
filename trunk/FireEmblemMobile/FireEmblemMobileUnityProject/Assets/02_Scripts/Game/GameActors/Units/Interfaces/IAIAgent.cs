using Game.AI;
using Game.GameActors.Players;

namespace Game.GameActors.Units
{
    public interface IAIAgent
    {
        AIComponent AIComponent { get; }
        GridComponent GridComponent { get; }
        
        Faction Faction { get; }
        
    }
}