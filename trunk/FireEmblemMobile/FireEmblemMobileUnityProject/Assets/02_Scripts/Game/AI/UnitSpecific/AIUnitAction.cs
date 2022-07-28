using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public struct AIUnitAction
    {
        public float Score;
        public Vector2Int Location;
        public IGridObject Target;
        public UnitActionType UnitActionType;
        public Unit Performer;

        public AIUnitAction(float score, Vector2Int loc, IGridObject target, UnitActionType type, Unit performer)
        {
            this.Score = score;
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