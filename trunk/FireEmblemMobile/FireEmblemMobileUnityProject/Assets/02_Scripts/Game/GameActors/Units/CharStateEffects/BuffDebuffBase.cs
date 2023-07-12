using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public abstract class BuffDebuffBase : ScriptableObject
    {
        [SerializeField] private int duration;
        public bool TakeEffect(Unit unit)
        {
            throw new System.NotImplementedException();
        }
    }
}