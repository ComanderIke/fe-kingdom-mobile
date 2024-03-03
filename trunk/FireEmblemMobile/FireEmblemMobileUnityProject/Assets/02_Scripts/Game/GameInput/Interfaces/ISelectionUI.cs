using UnityEngine;

namespace Game.GameInput.Interfaces
{
    public abstract class ISelectionUI : MonoBehaviour
    {
     
        public abstract void ShowUndo();

        public abstract void HideUndo();

    }
}