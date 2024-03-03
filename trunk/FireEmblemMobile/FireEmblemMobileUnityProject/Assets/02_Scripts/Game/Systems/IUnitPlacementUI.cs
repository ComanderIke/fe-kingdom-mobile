using System;
using System.Collections.Generic;
using Game.EncounterAreas.Encounters.Battle;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.Systems
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