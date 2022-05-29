using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GUI
{
    public abstract class IUnitPlacementUI:MonoBehaviour
    {
        public Action<List<Unit>> unitSelectionChanged;
        public abstract void Show(List<Unit> units, BattleMap chapter);
        public abstract void Hide();
        public Action OnFinished;
        [SerializeField]
        public BattleMap chapter;

   

    }
}