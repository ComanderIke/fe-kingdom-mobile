using UnityEngine;

namespace Game.GameInput
{
    public class SelectionUI : ISelectionUI
    {
        public GameObject UndoButton;
        public override void ShowUndo()
        {
           // Debug.Log("ShowUndo");
            UndoButton.SetActive(true);
        }

        public override void HideUndo()
        {
           // Debug.Log("HideUndo");
            UndoButton.SetActive(false);
        }
    }
}