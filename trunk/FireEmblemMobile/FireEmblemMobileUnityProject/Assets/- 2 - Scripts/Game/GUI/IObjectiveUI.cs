using Game.Grid;
using UnityEngine;

namespace Game.GUI
{
    public abstract class IObjectiveUI : MonoBehaviour
    {
        public abstract void Show(Chapter chapter);
        public abstract void Hide();
    }
}