﻿using Game.GameActors.Units;
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
            Tiles[c.GridComponent.GridPosition.X, c.GridComponent.GridPosition.Y].TileRenderer.StandOnVisual();
        }
       
        public void SetTileCastMaterial(Vector2 pos, FactionId playerId)
        {
            Tiles[(int) pos.x, (int) pos.y].SetCastMaterial(playerId);
           

        }
        public void SetFieldMaterial(Vector2 pos, FactionId playerId, bool activeUnit, bool activePlayer)
        {
            Tiles[(int) pos.x, (int) pos.y].SetMaterial(playerId, activeUnit,activePlayer);
           

        }
        public void SetFieldMaterialAttack(Vector2 pos, FactionId playerId, bool activeUnit,bool activePlayer)
        {
            Tiles[(int) pos.x, (int) pos.y].SetAttackMaterial(playerId, activeUnit,activePlayer);
           

        }
    }
}