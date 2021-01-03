using Game.GameActors.Units.OnGameObject;
using GameEngine;
using UnityEngine;

namespace Game.GameActors.Units
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