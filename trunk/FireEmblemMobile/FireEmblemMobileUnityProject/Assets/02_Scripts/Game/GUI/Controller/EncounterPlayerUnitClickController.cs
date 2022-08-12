using UnityEngine;

namespace _02_Scripts.Game.GUI.Controller
{
    public class EncounterPlayerUnitClickController : MonoBehaviour
    {
        [SerializeField] private EncounterPlayerUnitController encounterPlayerUnitController;

        private void OnMouseDown()
        {
            encounterPlayerUnitController.Clicked();
        }
    }
}