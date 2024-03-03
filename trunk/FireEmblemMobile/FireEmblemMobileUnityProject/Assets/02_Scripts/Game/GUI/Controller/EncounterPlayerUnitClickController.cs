using UnityEngine;

namespace Game.GUI.Controller
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