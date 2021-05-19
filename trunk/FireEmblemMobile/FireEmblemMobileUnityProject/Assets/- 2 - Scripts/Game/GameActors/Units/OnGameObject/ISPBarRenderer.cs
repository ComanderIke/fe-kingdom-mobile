using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public abstract class ISPBarRenderer:MonoBehaviour
    {
        public abstract void SetValue(int value, int maxValue);
    }
}