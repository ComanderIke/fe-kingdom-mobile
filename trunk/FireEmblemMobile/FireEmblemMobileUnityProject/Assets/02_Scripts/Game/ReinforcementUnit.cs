using Game.AI;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public enum ReinforcementTrigger
    {
        Turn,
        Area
    }
[System.Serializable]
    public struct ReinforcementUnit
    {
        [SerializeField] public UnitBP unitBp;
        public FactionId FactionId;
        public WeightSet AIWeightSet;
        public ReinforcementTrigger Trigger;
        public int turn;
        public ReinforcementTriggerArea area;
    }
}