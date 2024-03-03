using Game.AI.DecisionMaking;
using Game.GameActors.Factions;
using Game.GameActors.Units;
using UnityEngine;

namespace Game
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