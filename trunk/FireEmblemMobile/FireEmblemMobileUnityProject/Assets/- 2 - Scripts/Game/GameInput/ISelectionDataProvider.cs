using System.Numerics;
using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public interface ISelectionDataProvider
    {
        ISelectableActor SelectedActor { get; }

        Vector2Int GetSelectedTile();
        void SetSelectedTile(int x, int y);
        void SetSelectedAttackTarget(ISelectableActor target);
        ISelectableActor GetSelectedAttackTarget();
        void ClearData();
        bool IsSelectedTile(int i, int i1);
        void ClearAttackTarget();
    }
}