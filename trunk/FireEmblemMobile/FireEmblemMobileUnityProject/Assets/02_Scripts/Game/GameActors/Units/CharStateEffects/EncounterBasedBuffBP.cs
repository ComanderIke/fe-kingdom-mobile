using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class EncounterBasedBuffBP : ScriptableObject
    {
        [SerializeField] protected int duration;

        public abstract EncounterBasedBuff Create();
    }
    }