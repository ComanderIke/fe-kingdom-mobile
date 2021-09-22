using UnityEngine;

namespace Game.GameInput
{
    public class SelectionUI : ISelectionUI
    {
        public GameObject UndoButton;
        public override void ShowUndo()
        {
            UndoButton.SetActive(true);
        }

        public override void HideUndo()
        {
            UndoButton.SetActive(false);
        }
    }
}