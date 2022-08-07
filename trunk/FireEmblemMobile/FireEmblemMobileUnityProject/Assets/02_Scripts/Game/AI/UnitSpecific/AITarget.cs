using Game.GameActors.Units;

namespace Game.AI
{
    public class AITarget
    {
        public int Distance { get; set;  }
        public IGridActor Actor { get; set; }
    }
}