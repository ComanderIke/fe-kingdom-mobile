using Assets.Core;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameEngine;
using UnityEngine;

namespace Assets.GameActors.Units
{
    public class UnitsSystem : MonoBehaviour, IEngineSystem
    {
        private UnitInputController[] units;

        private void Start()
        {
            units = FindObjectsOfType<UnitInputController>();
        }

        public void ShowUnits()
        {
            foreach (var unit in units) unit.gameObject.SetActive(true);
        }

        public void HideUnits()
        {
            foreach (var unit in units) unit.gameObject.SetActive(false);
        }
    }
}