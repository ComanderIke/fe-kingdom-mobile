using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GUI
{
    public abstract class IUnitSelectionUI: MonoBehaviour
    {
        public Action<List<Unit>> unitSelectionChanged;
        public abstract void Show(List<Unit> units, List<Unit> selectedUnits);
        public abstract void Hide();
    }
}