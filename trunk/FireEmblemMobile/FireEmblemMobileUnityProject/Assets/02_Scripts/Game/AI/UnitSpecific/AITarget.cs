using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Grid.GridPathFinding;

namespace Game.AI
{
    public class AITarget
    {
        public int Distance { get; set;  }
        public MovementPath Path { get; set;  }
        public IGridObject TargetObject { get; set; }
    }
}