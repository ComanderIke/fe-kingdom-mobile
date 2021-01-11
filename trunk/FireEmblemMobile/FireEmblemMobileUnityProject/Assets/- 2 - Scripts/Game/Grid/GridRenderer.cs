using Game.GameActors.Units;
using Game.Map;
using UnityEngine;

namespace Game.Grid
{
    public class GridRenderer
    {
        public delegate void OnRenderEnemyTileEvent(int x, int y, Unit enemy, int playerId);
        public static event OnRenderEnemyTileEvent OnRenderEnemyTile;
        private Tile[,] Tiles { get; }

        public GridRenderer(GridSystem gridManager)
        {
            Tiles = gridManager.Tiles;
        }
        

        public void ShowStandOnVisual(IGridActor c)
        {
            Tiles[c.GridPosition.X, c.GridPosition.Y].TileRenderer.StandOnVisual();
        }
       
        public void SetFieldMaterial(Vector2 pos, int playerId)
        {
            Tiles[(int) pos.x, (int) pos.y].SetMaterial(playerId);
           

        }
        public void SetFieldMaterialAttack(Vector2 pos, int playerId)
        {
            Tiles[(int) pos.x, (int) pos.y].SetMaterialAttack();
           

        }
    }
}