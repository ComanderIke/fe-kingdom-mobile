using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GUI
{
    public abstract class IUnitPlacementUI:MonoBehaviour
    {
        public abstract void Show(List<Unit> units);
        public abstract void Hide();
    }
}