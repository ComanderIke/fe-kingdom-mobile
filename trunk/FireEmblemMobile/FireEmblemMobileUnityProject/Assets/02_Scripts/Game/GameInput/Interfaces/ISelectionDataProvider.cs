using Game.GameActors.Grid;
using Game.GameActors.Units.Interfaces;
using UnityEngine;

namespace Game.GameInput.Interfaces
{
    public interface ISelectionDataProvider
    {
        IGridActor SelectedActor { get; }

        Vector2Int GetSelectedTile();
        void SetSelectedTile(int x, int y);
        void SetSelectedAttackTarget(IGridObject target);
        IGridObject GetSelectedAttackTarget();
        void ClearData();
        bool IsSelectedTile(int i, int i1);
        void ClearAttackTarget();
        void SetUndoAbleActor(IGridActor selectedActor);
        void ClearPositionData();
        void ClearAttackData();
        IGridActor GetUndoAbleActor();
    }
}