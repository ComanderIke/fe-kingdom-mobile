using Game.GameActors.Grid;
using Game.Grid.GridPathFinding;

namespace Game.AI.UnitSpecific
{
    public class AITarget
    {
        public int Distance { get; set;  }
        public MovementPath Path { get; set;  }
        public IGridObject TargetObject { get; set; }
    }
}