using System;
using UnityEngine;

namespace Assets.GameActors.Units.CharStateEffects
{
    [Serializable]
    public abstract class CharacterState : ScriptableObject
    {
        private int currentDuration;
        public int Duration;

        public virtual bool TakeEffect(Unit unit)
        {
            if (currentDuration > 0)
                currentDuration -= 1;
            else
                return true;
            return false;
        }

        public abstract void Remove(Unit unit);

        private void OnEnable()
        {
            currentDuration = Duration;
        }
    }
}