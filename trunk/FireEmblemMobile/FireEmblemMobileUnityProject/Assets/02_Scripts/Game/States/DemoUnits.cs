using System;
using System.Collections.Generic;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.States
{
    public class DemoUnits:MonoBehaviour
    {
        [SerializeField]
        private List<UnitBP> units;
        [SerializeField]
        public BattleMap battleMap;

        public int StartAreaIndex = 0;
        // [SerializeField]
        // private List<StockedItem> convoy;

        void Start()
        {
            //Debug.LogError("TESTERROR");
            // Player.Instance.Party.convoy = convoy;
        }
        public List<Unit> GetUnits()
        {
            var list = new List<Unit>();
            foreach (var unit in units)
            {
                list.Add(unit.Create(Guid.NewGuid()));
            }

            return list;
        }
    }
}