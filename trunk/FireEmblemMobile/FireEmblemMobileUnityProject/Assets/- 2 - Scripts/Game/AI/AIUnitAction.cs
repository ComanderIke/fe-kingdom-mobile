using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public class AIUnitAction
    {
        public float Score;
        public Vector2Int Location;
        public IGridObject Target;
        public UnitAction UnitAction;
        public Unit Performer;
    }

    public enum UnitAction
    {
        Wait,
        Attack,
        Heal
    }
}