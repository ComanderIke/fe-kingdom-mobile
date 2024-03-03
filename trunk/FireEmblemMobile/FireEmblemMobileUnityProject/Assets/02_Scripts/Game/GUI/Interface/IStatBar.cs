using UnityEngine;

namespace Game.GUI.Interface
{
    public abstract class IStatBar: MonoBehaviour
    {
        public abstract void SetValue(int value, int maxValue, bool animated);
    }
}