﻿using Assets.Core;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using Assets.GUI;
using Assets.Map;
using System;
using UnityEngine;

namespace Assets.Grid
{
    public class GridRenderer
    {
        public delegate void OnRenderEnemyTileEvent(int x, int y, Unit enemy, int playerId);
        public static event OnRenderEnemyTileEvent OnRenderEnemyTile;
        public MapSystem GridManager { get; set; }
        public Tile[,] Tiles { get; set; }

        public GridRenderer(MapSystem gridManager)
        {
            GridManager = gridManager;
            Tiles = gridManager.Tiles;
        }

        public void SetBigTileActive(BigTile bigTile, int playerId, bool attack)
        {
            SetFieldMaterial(bigTile.BottomLeft(), playerId, attack);
            SetFieldMaterial(bigTile.BottomRight(), playerId, attack);
            SetFieldMaterial(bigTile.TopLeft(), playerId, attack);
            SetFieldMaterial(bigTile.TopRight(), playerId, attack);
        }

        public void ShowStandOnTexture(Unit c)
        {
            if (c.GridPosition is BigTilePosition bigTile)
            {
                Tiles[(int) bigTile.Position.BottomLeft().x, (int) bigTile.Position.BottomLeft().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
                Tiles[(int) bigTile.Position.BottomRight().x, (int) bigTile.Position.BottomRight().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
                Tiles[(int) bigTile.Position.TopLeft().x, (int) bigTile.Position.TopLeft().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
                Tiles[(int) bigTile.Position.TopRight().x, (int) bigTile.Position.TopRight().y].GameObject
                    .GetComponent<MeshRenderer>().material = GridManager.GridResources.CellMaterialStandOn;
            }
            else
                Tiles[c.GridPosition.X, c.GridPosition.Y].GameObject.GetComponent<MeshRenderer>().material =
                    GridManager.GridResources.CellMaterialStandOn;
        }
        private void SetAttackFieldMaterial(Vector2 pos, int playerId, bool soft)
        {
            var meshRenderer = Tiles[(int)pos.x, (int)pos.y].GameObject.GetComponent<MeshRenderer>();
            if (soft)
                meshRenderer.material = GridManager.GridResources.CellMaterialSoftAttack;
            else
            {
                meshRenderer.material = GridManager.GridResources.CellMaterialAttack;
                OnRenderEnemyTile?.Invoke((int)pos.x, (int)pos.y, Tiles[(int)pos.x, (int)pos.y].Unit, playerId);
            }
            
   
        }
        public void SetFieldMaterial(Vector2 pos, int playerId, bool attack, bool soft=false)
        {
            var meshRenderer = Tiles[(int) pos.x, (int) pos.y].GameObject.GetComponent<MeshRenderer>();
            if (attack)
            {
                //not using sharedMaterial here create Material instances which will cause much higher batches
                if (meshRenderer.sharedMaterial == GridManager.GridResources.CellMaterialSoftMovement || meshRenderer.sharedMaterial == GridManager.GridResources.CellMaterialMovement|| meshRenderer.sharedMaterial == GridManager.GridResources.CellMaterialAlly)
                    return;
                SetAttackFieldMaterial(pos, playerId, soft);
            }
            else
            {
                if (Tiles[(int)pos.x, (int)pos.y].Unit != null && Tiles[(int)pos.x, (int)pos.y].Unit.Faction.Id == playerId)
                    meshRenderer.material = GridManager.GridResources.CellMaterialAlly;
                else
                {
                    if(soft)
                        meshRenderer.material = GridManager.GridResources.CellMaterialSoftMovement;
                    else
                        meshRenderer.material = GridManager.GridResources.CellMaterialMovement;
                }
                if (Tiles[(int) pos.x, (int) pos.y].Unit != null &&
                    Tiles[(int) pos.x, (int) pos.y].Unit.Faction.Id != playerId)
                {
                    SetAttackFieldMaterial(pos, playerId,soft);
                }

                Tiles[(int) pos.x, (int) pos.y].IsActive = true;
            }
        }
    }
}