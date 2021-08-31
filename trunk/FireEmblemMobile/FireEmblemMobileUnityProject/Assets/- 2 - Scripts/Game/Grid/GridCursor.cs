using System;
using Game.Grid;
using UnityEngine;

namespace Game.Map
{
    public class GridCursor
    {
        private GridPosition gridPosition;

        private Tile currentTile;
        private GridCursorRenderer renderer;

        public GridCursor()
        {
            renderer = GameObject.FindObjectOfType<GridCursorRenderer>();
            renderer.Hide();
        }
        public Tile GetCurrentTile()
        {
            return currentTile;
        }
        public void SetCurrentTile(Tile tile)
        {
            currentTile = tile;
            gridPosition = new GridPosition(currentTile.X, currentTile.Y);
            OnCursorPositionChanged?.Invoke(new Vector2Int(gridPosition.X,gridPosition.Y));
            renderer.Show(new Vector2(gridPosition.X,gridPosition.Y));
        }

        public event Action<Vector2Int> OnCursorPositionChanged;
        //OnResetCursor?

        public void Reset()
        {
            currentTile = null;
            gridPosition = null;
           // renderer.Hide();
            //OnResetCursor
        }
    }
}