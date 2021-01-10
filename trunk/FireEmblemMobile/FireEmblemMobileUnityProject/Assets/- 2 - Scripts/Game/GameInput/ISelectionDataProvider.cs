using System.Numerics;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GameInput
{
    public interface ISelectionDataProvider
    {
        IGridActor SelectedActor { get; }

        Vector2Int GetSelectedTile();
        void SetSelectedTile(int x, int y);
        IGridActor ConfirmAttackTarget { get; set; }
        void ClearSelectedTile();
        bool IsSelectedTile(int i, int i1);
    }
}