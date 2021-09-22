using Game.GameActors.Units;
using Game.Manager;
using Game.Mechanics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Game.GameInput
{
    public class SelectionManager : ISelectionDataProvider
    {
        private UnitSelectionSystem unitSelectionSystem;
        public IGridActor SelectedActor => unitSelectionSystem.SelectedCharacter;
        private int SelectedTileX { get; set; }
        private int SelectedTileY { get; set; }
        private IGridActor selectedAttackTarget;
        private IGridActor undoAbleActor;
        private ISelectionUI selectionUI;

        public void SetSelectedAttackTarget(IGridActor target)
        {
            selectedAttackTarget?.SetAttackTarget(false);
            selectedAttackTarget = target;
            target?.SetAttackTarget(true);
        }

        public IGridActor GetSelectedAttackTarget()
        {
            return selectedAttackTarget;
        }

        public void ClearData()
        {
            SelectedTileX = -1;
            SelectedTileY = -1;
            ClearAttackTarget();
            undoAbleActor = null;
        }
        public void ClearAttackTarget()
        {
            SetSelectedAttackTarget(null);
            undoAbleActor = null;
            selectionUI.HideUndo();
        }

        public void SetUndoAbleActor(IGridActor selectedActor)
        {
            undoAbleActor = selectedActor;
            selectionUI.ShowUndo();
        }

        public bool IsSelectedTile(int x, int y)
        {
            return SelectedTileX == x && SelectedTileY == y;
        }

        public SelectionManager()
        {
            unitSelectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            selectionUI = GameObject.FindObjectOfType<SelectionUI>();
            SelectedTileX = -1;
            SelectedTileY = -1;
        }

        public void SetSelectedTile(int x, int y)
        {
            SelectedTileX = x;
            SelectedTileY = y;
        }

        public Vector2Int GetSelectedTile()
        {
            return new Vector2Int(SelectedTileX, SelectedTileY);
        }
    }
}