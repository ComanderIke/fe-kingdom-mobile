using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.States
{
    public class DemoUnits:MonoBehaviour
    {
        [SerializeField]
        private List<Unit> units;

        void Start()
        {
            Debug.LogError("TESTERROR");
        }
        public List<Unit> GetUnits()
        {
            var list = new List<Unit>();
            foreach (var unit in units)
            {
                list.Add(Instantiate(unit));
            }

            return list;
        }
    }
}