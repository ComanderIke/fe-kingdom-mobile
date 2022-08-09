using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public struct AIUnitAction
    {
        public Vector2Int Location;
        public IAttackableTarget Target;
        public UnitActionType UnitActionType;
        public IAIAgent Performer;

        public AIUnitAction(Vector2Int loc, IAttackableTarget target, UnitActionType type, IAIAgent performer)
        {
            this.Location = loc;
            this.Target = target;
            this.UnitActionType = type;
            this.Performer = performer;
        }
    }

    public enum UnitActionType
    {
        Wait,
        Attack,
        Heal
    }
}