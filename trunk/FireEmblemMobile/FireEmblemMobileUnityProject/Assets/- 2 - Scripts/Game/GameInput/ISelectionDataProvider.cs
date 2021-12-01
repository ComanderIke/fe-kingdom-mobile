using System.Numerics;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public interface ISelectionDataProvider
    {
        IGridActor SelectedActor { get; }

        Vector2Int GetSelectedTile();
        void SetSelectedTile(int x, int y);
        void SetSelectedAttackTarget(IGridActor target);
        IGridActor GetSelectedAttackTarget();
        void ClearData();
        bool IsSelectedTile(int i, int i1);
        void ClearAttackTarget();
        void SetUndoAbleActor(IGridActor selectedActor);
        void ClearPositionData();
        void ClearAttackData();
        IGridActor GetUndoAbleActor();
    }
}