using Game.Grid;
using UnityEngine;

namespace Game.GUI
{
    public abstract class IObjectiveUI : MonoBehaviour
    {
        public abstract void Show(BattleMap chapter);
        public abstract void Hide();
    }
}