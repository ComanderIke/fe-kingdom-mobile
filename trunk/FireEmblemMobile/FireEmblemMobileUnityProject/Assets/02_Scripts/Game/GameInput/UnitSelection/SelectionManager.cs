using Game.GameActors.Players;
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
        private IGridObject selectedAttackTarget;
        private IGridActor undoAbleActor;
        private ISelectionUI selectionUI;

        public void SetSelectedAttackTarget(IGridObject target)
        {
            selectedAttackTarget?.SetAttackTarget(false);
            selectedAttackTarget = target;
            target?.SetAttackTarget(true);
        }

        public IGridObject GetSelectedAttackTarget()
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
        public void ClearPositionData()
        {
            SelectedTileX = -1;
            SelectedTileY = -1;
           
        }
        public void ClearAttackData(){
            SetSelectedAttackTarget(null);
        }
        public void ClearAttackTarget()
        {
            SetSelectedAttackTarget(null);
            undoAbleActor = null;
            //selectionUI.HideUndo();
        }

        public void SetUndoAbleActor(IGridActor selectedActor)
        {
            if (SelectedActor.TurnStateManager.HasCantoed || SelectedActor.TurnStateManager.HasAttacked)
                return;
                
            undoAbleActor = selectedActor;
           
            selectionUI.ShowUndo();
        }
        public IGridActor GetUndoAbleActor()
        {
            return undoAbleActor;
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