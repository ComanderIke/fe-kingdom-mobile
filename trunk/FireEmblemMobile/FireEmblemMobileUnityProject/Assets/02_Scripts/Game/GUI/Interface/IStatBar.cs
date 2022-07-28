using UnityEngine;

namespace Game.GUI
{
    public abstract class IStatBar: MonoBehaviour
    {
        public abstract void SetValue(int value, int maxValue);
    }
}