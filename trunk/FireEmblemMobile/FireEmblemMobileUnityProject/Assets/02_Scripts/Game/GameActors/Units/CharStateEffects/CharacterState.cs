using System;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    [Serializable]
    public abstract class CharacterState : ScriptableObject
    {
        private int currentDuration;
        public int Duration;
        public GameObject Visual;

        public virtual bool TakeEffect(Unit unit)
        {
            if (currentDuration > 0)
                currentDuration -= 1;
            else
                return true;
            return false;
        }
        

        private void OnEnable()
        {
            currentDuration = Duration;
        }
    }
}