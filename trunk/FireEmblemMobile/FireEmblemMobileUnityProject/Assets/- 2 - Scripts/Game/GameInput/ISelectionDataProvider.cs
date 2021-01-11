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
        ISelectableActor selectedAttackTarget { get; set; }
        void ClearSelectedTile();
        bool IsSelectedTile(int i, int i1);
    }
}