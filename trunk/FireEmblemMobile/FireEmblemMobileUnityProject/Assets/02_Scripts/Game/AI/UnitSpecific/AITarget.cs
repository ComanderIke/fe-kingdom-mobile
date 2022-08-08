using Game.GameActors.Units;
using Game.Grid.GridPathFinding;

namespace Game.AI
{
    public class AITarget
    {
        public int Distance { get; set;  }
        public MovementPath Path { get; set;  }
        public IGridActor Actor { get; set; }
    }
}