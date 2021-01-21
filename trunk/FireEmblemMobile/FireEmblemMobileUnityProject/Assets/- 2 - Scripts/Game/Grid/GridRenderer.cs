using Game.GameActors.Units;
using Game.Map;
using UnityEngine;

namespace Game.Grid
{
    public class GridRenderer
    {

        private Tile[,] Tiles { get; }

        public GridRenderer(GridSystem gridManager)
        {
            Tiles = gridManager.Tiles;
        }
        

        public void ShowStandOnVisual(IGridActor c)
        {
            Tiles[c.GridPosition.X, c.GridPosition.Y].TileRenderer.StandOnVisual();
        }
       
        public void SetFieldMaterial(Vector2 pos, int playerId, bool activeUnit)
        {
            Tiles[(int) pos.x, (int) pos.y].SetMaterial(playerId, activeUnit);
           

        }
        public void SetFieldMaterialAttack(Vector2 pos, int playerId, bool activeUnit)
        {
            Tiles[(int) pos.x, (int) pos.y].SetAttackMaterial(playerId, activeUnit);
           

        }
    }
}