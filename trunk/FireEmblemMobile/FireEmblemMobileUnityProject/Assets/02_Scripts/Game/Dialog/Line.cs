using Game.GameActors.Units;
using UnityEngine;

namespace Game.Dialog
{
    [System.Serializable]
    public class Line
    {
        public string sentence;
        [SerializeField] UnitBP actor;
        public bool left = true;
        public UnitBP Actor => actor;
    }
}