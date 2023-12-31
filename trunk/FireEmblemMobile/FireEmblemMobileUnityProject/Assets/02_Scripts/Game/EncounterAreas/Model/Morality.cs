using System;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    [System.Serializable]
    public class Morality
    {
        public event Action<float, float> OnMoralityChanged;
        private float morality = 0;

        public void AddMorality(float add) // make it harder to gain morality if its close to 1 and harder to lose morality if close to -1?
        {
            MyDebug.LogLogic("Add Morality: "+add);
            if (add == 0)
                return;
            morality += add;
            if (morality < -100)
            {
                morality = -100;
            }
            else if (morality > 100)
                morality = 100;
            OnMoralityChanged?.Invoke(morality, add);
        }
        public float GetCurrentMoralityValue() // -1 to 1 or 0 to 1? with 0.5 being neutral
        {
            return morality;
        }

        public void Set(float f)
        {
            morality = f;
        }
    }
}